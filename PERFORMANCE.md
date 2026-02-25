# Performance Improvement Recommendations

Analysis of the Wrap codebase for clear, actionable performance improvements.

---

## 1. Double Enumeration in `IEnumerable<T>` Extensions (Critical)

The biggest issue in the codebase: four `EnumerableExtensions` methods enumerate the source collection **twice** — once to check a condition and again to retrieve the value. When callers pass a lazily-evaluated `IEnumerable<T>` (e.g. a LINQ chain or database query), this doubles the work.

### 1a. `SingleOrNone` — `Count()` + `Single()`

**File:** `src/All/Extensions/Enumerable/EnumerableExtensions.SingleOrNone.cs:21-29`

```csharp
// Current: enumerates fully for Count(), then again for Single()
public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this) =>
    @this.Count() switch
    {
        1 => @this.Single(),
        _ => M.None
    };
```

**Fix:** Use an enumerator to check for exactly one element in a single pass:

```csharp
public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> @this)
{
    using var e = @this.GetEnumerator();
    if (!e.MoveNext())
        return M.None;

    var value = e.Current;
    return e.MoveNext() ? M.None : value;
}
```

### 1b. `ElementAtOrNone` — `Count()` + `ElementAt()`

**File:** `src/All/Extensions/Enumerable/EnumerableExtensions.ElementAtOrNone.cs:18-26`

```csharp
// Current: enumerates fully for Count(), then re-enumerates up to index
public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index) =>
    (@this.Count() > index) switch
    {
        true => @this.ElementAt(index),
        false => M.None
    };
```

**Fix:** Use a try-pattern or manual enumerator skip:

```csharp
public static Maybe<T> ElementAtOrNone<T>(this IEnumerable<T> @this, int index)
{
    if (index < 0)
        return M.None;

    using var e = @this.GetEnumerator();
    for (var i = 0; i <= index; i++)
    {
        if (!e.MoveNext())
            return M.None;
    }

    return e.Current;
}
```

### 1c. `FirstOrNone` — `Any()` + `First()`

**File:** `src/All/Extensions/Enumerable/EnumerableExtensions.FirstOrNone.cs:17-25`

```csharp
// Current: enumerates once for Any(), then again for First()
public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this) =>
    @this.Any() switch
    {
        true => @this.First(),
        _ => M.None
    };
```

**Fix:** Single-pass with the enumerator:

```csharp
public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> @this)
{
    using var e = @this.GetEnumerator();
    return e.MoveNext() ? e.Current : M.None;
}
```

### 1d. `LastOrNone` — `Any()` + `Last()`

**File:** `src/All/Extensions/Enumerable/EnumerableExtensions.LastOrNone.cs:17-25`

```csharp
// Current: enumerates once for Any(), then again for Last()
public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this) =>
    @this.Any() switch
    {
        true => @this.Last(),
        _ => M.None
    };
```

**Fix:** Single-pass, keeping track of the last value:

```csharp
public static Maybe<T> LastOrNone<T>(this IEnumerable<T> @this)
{
    using var e = @this.GetEnumerator();
    if (!e.MoveNext())
        return M.None;

    var last = e.Current;
    while (e.MoveNext())
        last = e.Current;

    return last;
}
```

---

## 2. Multiple Enumeration in `GetSingle` Pattern Matching (High)

**File:** `src/All/Extensions/Result/ResultExtensions.GetSingle.cs:36-56`

The pattern matching chain calls `list.Count() == 1`, then falls through to `!list.Any()` — each of which fully or partially enumerates:

```csharp
IEnumerable<TSingle> list when list.Count() == 1 =>    // full enumeration
    R.Wrap(list.Single()),                               // second full enumeration

IEnumerable<TSingle> list when !list.Any() =>            // third enumeration attempt
    ...
```

**Fix:** Materialise once or use an enumerator-based check, similar to the `SingleOrNone` approach above. For example:

```csharp
IEnumerable<TSingle> list => list.SingleOrNone().Match(
    fNone: () => !list.Any()
        ? (onError?.Invoke(C.GetSingle.EmptyList) ?? R.Fail(C.GetSingle.EmptyList)
            .Ctx(nameof(ResultExtensions), nameof(GetSingle)))
        : (onError?.Invoke(C.GetSingle.MultipleValues) ?? R.Fail(C.GetSingle.MultipleValues)
            .Ctx(nameof(ResultExtensions), nameof(GetSingle))),
    fSome: v => R.Wrap(v)
),
```

Or better still, use an enumerator-based helper that returns a discriminated result (empty / single / multiple) from a single pass, then match on that.

---

## 3. Unnecessary Async State Machine Allocations (High)

Across many files, synchronous functions are wrapped in `async` lambdas purely to match an `Func<T, Task<TReturn>>` parameter:

```csharp
// This pattern appears ~130 times across the codebase
BindAsync(@this, async x => f(x));         // allocates a state machine for no reason
FilterAsync(@this, async x => fTest(x));
MapAsync(@this, async x => f(x));
```

Each `async x => f(x)` allocates a compiler-generated state machine and a `Task<T>` wrapper around what is fundamentally a synchronous call.

**Fix:** Use `Task.FromResult` to return synchronously:

```csharp
BindAsync(@this, x => Task.FromResult(f(x)));
```

Or provide dedicated synchronous-to-async bridge overloads that avoid the async machinery entirely. The files most affected are:

- `EnumerableExtensions.Bind.cs:36,77`
- `EnumerableExtensions.Map.cs:36,77`
- `EnumerableExtensions.Filter.cs:71,117`
- `EnumerableExtensions.FilterMap.cs:49,109`
- `EnumerableExtensions.FilterBind.cs:49,109`
- `EnumerableExtensions.Iterate.cs:38,80`
- `ResultExtensions.MatchIf.cs` (~50 instances)
- `MaybeExtensions.MatchIf.cs` (~50 instances)
- `E.Match.cs`, `M.Match.cs`, `R.Match.cs` (multiple instances each)

---

## 4. Reflection Without Caching in `F.Format` (Medium)

**File:** `src/Common/Functions/F.Format.cs:99`

Each template placeholder triggers a `typeof(T).GetProperty(template, flags)` call inside the regex replace lambda. For format strings with multiple named placeholders (or repeated calls with the same type), this performs repeated reflection lookups:

```csharp
{ } obj when !numberedTemplates && typeof(T).GetProperty(template, flags)?.GetValue(obj) is object val =>
    val,
```

**Fix:** Cache property lookups per type using a `ConcurrentDictionary<(Type, string), PropertyInfo?>`:

```csharp
private static readonly ConcurrentDictionary<(Type, string), PropertyInfo?> _propertyCache = new();

// In the lambda:
var prop = _propertyCache.GetOrAdd(
    (typeof(T), template),
    key => key.Item1.GetProperty(key.Item2, flags)
);
```

---

## 5. Temporary Allocations in `F.Format` (Low)

**File:** `src/Common/Functions/F.Format.cs:65,110-113,117`

Several minor allocations in the format method:

1. **`new List<object>()`** (line 65) grows dynamically — consider initialising with capacity based on a quick count of placeholders.
2. **`new string('{', count)` and `new string('}', count)`** (lines 110,113) — for the common case of single braces, these allocate a new string each time. Could use pre-allocated constants for count == 1.
3. **`[.. values]`** (line 117) — the spread operator creates a new array from the list. Using `values.ToArray()` is equivalent but `CollectionsMarshal.AsSpan` or passing the list directly could avoid the copy on newer .NET versions.

---

## Summary

| Priority | Issue | Impact | Location |
|----------|-------|--------|----------|
| Critical | Double enumeration in `SingleOrNone` | 2x work on every call | `EnumerableExtensions.SingleOrNone.cs` |
| Critical | Double enumeration in `ElementAtOrNone` | 2x work on every call | `EnumerableExtensions.ElementAtOrNone.cs` |
| Critical | Double enumeration in `FirstOrNone` | 2x work on every call | `EnumerableExtensions.FirstOrNone.cs` |
| Critical | Double enumeration in `LastOrNone` | 2x work on every call | `EnumerableExtensions.LastOrNone.cs` |
| High | Multiple enumeration in `GetSingle` | Up to 3x enumeration | `ResultExtensions.GetSingle.cs` |
| High | `async x => f(x)` wrappers (~130 sites) | Unnecessary state machine + Task allocation per call | Many files (see Section 3) |
| Medium | Uncached reflection in `F.Format` | Repeated `GetProperty` per placeholder | `F.Format.cs` |
| Low | Temporary allocations in `F.Format` | Minor heap pressure | `F.Format.cs` |
