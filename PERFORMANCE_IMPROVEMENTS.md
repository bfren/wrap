# Performance Improvement Recommendations

Analysis of the Wrap library codebase (`Maybe<T>`, `Result<T>`, `Either`, `Monad<T>`) identifying clear, actionable performance improvements ordered by impact.

---

## 1. Double-enumeration in `FirstOrNone`, `LastOrNone`, `SingleOrNone`, `ElementAtOrNone`

**Files:**
- `src/All/Extensions/Enumerable/EnumerableExtensions.FirstOrNone.cs`
- `src/All/Extensions/Enumerable/EnumerableExtensions.LastOrNone.cs`
- `src/All/Extensions/Enumerable/EnumerableExtensions.SingleOrNone.cs`
- `src/All/Extensions/Enumerable/EnumerableExtensions.ElementAtOrNone.cs`

Each method calls two separate LINQ operations on the same `IEnumerable<T>`, enumerating it twice:

```csharp
// FirstOrNone — Any() then First() = two passes
public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
    @this.Any() switch
    {
        true => @this.First(),
        _    => M.None
    };

// SingleOrNone — Count() then Single() = two full passes
// ElementAtOrNone — Count() then ElementAt() = two full passes
// LastOrNone — Any() then Last() = two full passes
```

For lazy sequences (generators, LINQ chains, EF query results) this enumerates the source **twice**, which is both inefficient and semantically incorrect (a generator may produce different elements on each pass).

**Fix:** Use LINQ's built-in fallback methods with a null/default sentinel:

```csharp
public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this)
{
    using var e = @this.GetEnumerator();
    return e.MoveNext() ? M.Wrap(e.Current) : M.None;
}

// Or for reference types relying on M.Wrap's null check:
public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
    M.Wrap(@this.FirstOrDefault()!);
```

---

## 2. `NoneImpl` heap-allocated on every `None` return

**File:** `src/Maybe/Maybe.Operators.cs:20`, `src/Maybe/Functions/M.None.cs:22`

```csharp
// Every assignment of M.None to a Maybe<T> triggers this:
public static implicit operator Maybe<T>(None _) =>
    new NoneImpl();  // heap allocation every single time
```

`NoneImpl` is stateless — its only field is the `None` struct which is always identical. The same instance can be safely reused per `T` with a static singleton:

```csharp
private static readonly NoneImpl _none = new();

public static implicit operator Maybe<T>(None _) => _none;
```

Similarly, `M.NoneAsTask<T>()` creates `new NoneImpl()` and wraps it in `Task.FromResult` on every call. A cached `Task<Maybe<T>>` per `T` would eliminate both allocations.

---

## 3. Unnecessary closure allocations in void `Match` overloads

**Files:** `src/Maybe/Functions/M.Match.cs:32–50`, `src/Result/Functions/R.Match.cs:32–50`

The void `Match` overloads assign the branch result to a delegate variable and then invoke it, which allocates a closure on every `Some`/`Ok` arm:

```csharp
var f = maybe switch
{
    Maybe<T>.NoneImpl => fNone,
    Some<T> x         => () => fSome(x.Value),  // closure allocates on heap
    ...
};
f();
```

The `Some<T>` arm captures both `fSome` and `x` into a new delegate object. Since `f()` is called unconditionally right after, the indirection is purely structural. Direct branching avoids the allocation entirely:

```csharp
switch (maybe)
{
    case Maybe<T>.NoneImpl: fNone(); return;
    case Some<T> x:         fSome(x.Value); return;
    case { } m:             throw new InvalidMaybeTypeException(m.GetType());
    default:                throw new NullMaybeException();
}
```

---

## 4. `Task.FromResult` wrapping in async bridge overloads

**Files:** `src/Maybe/Functions/M.Match.cs:53–74`, `src/Result/Functions/R.Match.cs:52–74`

Many `MatchAsync` overloads that receive a synchronous `Maybe<T>` or `Result<T>` adapt to the fully-async overload by wrapping in `Task.FromResult`:

```csharp
// M.Match.cs:61
public static Task MatchAsync<T>(Maybe<T> maybe, Func<Task> fNone, Func<T, Task> fSome) =>
    MatchAsync(Task.FromResult(maybe), fNone, fSome);  // allocates Task<Maybe<T>>
```

`Task.FromResult` allocates a new `Task<T>` object. In tight monadic pipelines with many short-lived chains this adds steady allocation pressure. Each such overload should be given a direct implementation that works on the already-resolved value without boxing it into a task.

---

## 5. `WrapCache` sync methods block on async (deadlock risk + thread waste)

**File:** `src/Caching/WrapCache.cs:116`

```csharp
public Maybe<TValue> GetOrCreate<TValue>(TKey key, Func<Maybe<TValue>> valueFactory, MemoryCacheEntryOptions opt) =>
    GetOrCreateAsync(key, async () => valueFactory(), opt).Result;  // blocks calling thread
```

`.Result` on an async method blocks the calling thread for the entire duration of the async operation. This wastes a thread-pool thread and **can deadlock** in any `SynchronizationContext` that does not permit re-entrant awaits (ASP.NET Framework, WinForms, WPF).

The synchronous `GetOrCreate` path should have a dedicated synchronous implementation. Inside that path `CacheLock.WaitAsync()` becomes `CacheLock.Wait()` and the `await` becomes a direct call.

---

## 6. Uncached reflection in `F.GetGenericTypeArguments`

**File:** `src/Common/Functions/F.GetGenericTypeArguments.cs:17`

```csharp
internal static Type[] GetGenericTypeArguments(Type type, Type genericType) =>
    [..
        from i in type.GetInterfaces()      // reflection call, not cached
        where i.IsGenericType && i.GetGenericTypeDefinition() == genericType
        from a in i.GenericTypeArguments
        select a
    ];
```

`type.GetInterfaces()` is a reflection call that enumerates every interface implemented by the type. This is invoked on every call to `F.GetMonadTypes` and `F.GetMonadValueType`, which are used by the JSON converter factory and the MVC model binder providers.

A `ConcurrentDictionary<(Type, Type), Type[]>` keyed on `(type, genericType)` would make all repeat lookups O(1):

```csharp
private static readonly ConcurrentDictionary<(Type, Type), Type[]> _cache = new();

internal static Type[] GetGenericTypeArguments(Type type, Type genericType) =>
    _cache.GetOrAdd((type, genericType), static key =>
        [.. from i in key.Item1.GetInterfaces()
            where i.IsGenericType && i.GetGenericTypeDefinition() == key.Item2
            from a in i.GenericTypeArguments
            select a]);
```

---

## 7. `F.IsNullableValueType` — repeated reflection per call

**File:** `src/Common/Functions/F.IsNullableValueType.cs:17`

```csharp
public static bool IsNullableValueType<T>(T _) =>
    typeof(T) switch
    {
        Type t when t.IsValueType && Nullable.GetUnderlyingType(t) is not null => true,
        _ => false
    };
```

`Nullable.GetUnderlyingType` is a reflection call. Since `T` is a compile-time generic parameter, the result is constant for a given `T`. A static generic helper caches the result with zero overhead after the first call:

```csharp
private static class NullableHelper<T>
{
    public static readonly bool IsNullable =
        typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) is not null;
}

public static bool IsNullableValueType<T>(T _) => NullableHelper<T>.IsNullable;
```

This is called in `F.Wrap<TMonad,TValue>` and `Monad<TMonad,TValue>.Check` — both are hot paths invoked every time a monad value is set.

---

## 8. `GetHashCode` and equality operators route through `M.Match` / `R.Match`

**Files:** `src/Maybe/Maybe.Equatable.cs`, `src/Result/Result.Equatable.cs`

```csharp
// Called on every dictionary lookup, HashSet add, LINQ Distinct, etc.
public override int GetHashCode() =>
    M.Match(this,
        fNone: M.None.GetHashCode,
        fSome: x => x.GetHashCode()   // delegate allocation
    );

public static bool operator ==(Maybe<T> l, T r) =>
    M.Match(l,
        fNone: () => false,            // delegate allocation
        fSome: x => Equals(x, r)      // delegate allocation
    );
```

`GetHashCode` and `==`/`!=` are called constantly by `Dictionary<K,V>`, `HashSet<T>`, and LINQ. Routing each call through the `M.Match` dispatcher (a switch expression + two delegate parameters, each potentially allocating a closure) is unnecessary overhead. Direct `is` checks are both simpler and faster:

```csharp
public override int GetHashCode() =>
    this is Some<T> s ? s.Value?.GetHashCode() ?? 0 : 0;

public static bool operator ==(Maybe<T> l, T r) =>
    l is Some<T> s && Equals(s.Value, r);
```

---

## 9. `MonadJsonConverter.Write` — intermediate `byte[]` allocation

**File:** `src/All/Json/MonadJsonConverter.cs:63`

```csharp
var json = JsonSerializer.SerializeToUtf8Bytes(value.Value, options);  // allocates byte[]
writer.WriteRawValue(json);
```

`SerializeToUtf8Bytes` serialises the entire value to an intermediate `byte[]` before writing. Writing directly to the `Utf8JsonWriter` avoids the intermediate allocation:

```csharp
JsonSerializer.Serialize(writer, value.Value, options);
```

---

## 10. `F.Format` — per-call property reflection for named placeholders

**File:** `src/Common/Functions/F.Format.cs:99`

```csharp
{ } obj when !numberedTemplates
    && typeof(T).GetProperty(template, flags)?.GetValue(obj) is object val =>
    val,
```

`GetProperty` is called inside the regex-replace loop — once per placeholder per `Format` invocation. For repeated calls with the same type `T` and format string, all property lookups are redundant.

Caching a `Dictionary<string, PropertyInfo>` per `typeof(T)` (or a compiled expression/delegate per `(Type, string)` pair) would reduce per-call reflection to zero after warmup.

---

## Summary

| # | Issue | File(s) | Impact |
|---|-------|---------|--------|
| 1 | Double enumeration in `FirstOrNone` etc. | `EnumerableExtensions.*.cs` | **High** |
| 2 | `NoneImpl` allocated on every `None` return | `Maybe.Operators.cs` | **High** |
| 3 | Closure allocation in void `Match` overloads | `M.Match.cs`, `R.Match.cs` | **High** |
| 4 | `Task.FromResult` in async bridge overloads | `M.Match.cs`, `R.Match.cs` | **Medium–High** |
| 5 | Sync blocking on async in `WrapCache` | `WrapCache.cs` | **Medium** (+ correctness risk) |
| 6 | Uncached reflection in `GetGenericTypeArguments` | `F.GetGenericTypeArguments.cs` | **Medium** |
| 7 | `IsNullableValueType` repeated reflection | `F.IsNullableValueType.cs` | **Medium** |
| 8 | `GetHashCode`/operators routed via `M.Match` | `Maybe.Equatable.cs`, `Result.Equatable.cs` | **Medium** |
| 9 | Intermediate `byte[]` in `MonadJsonConverter.Write` | `MonadJsonConverter.cs` | **Low–Medium** |
| 10 | Per-call property reflection in `F.Format` | `F.Format.cs` | **Low–Medium** |
