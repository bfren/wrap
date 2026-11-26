# API review

Review of the public API of `main` (`b59c21f`, 2.0.0-beta.5 plus #144), 26 November 2026, before 2.0.0.

The six audits in `audit-plan.md` looked for bugs. This review asks a different question: what in the public API would you regret having to keep once 2.0.0 ships, when changing it needs a 3.0? It covers names, overload sets, what is public, and consistency between Maybe, Result and Either.

**How it was done:** the public surface of all 14 packages was dumped from the built Release assemblies (about 1,000 members). The same was done for Wrap 1.4.0, to see what is new in 2.0 and what came over from 1.x. Every item marked *reproduced* was compiled in a consumer project outside the repo, against packages packed from `main`.

**Why now:** 2.0 already breaks 1.x code in many places (`FailureValue` became `Failure`, the union types, `MatchIf` argument order). Fixing these items in 2.0 adds to a migration users are already making. After 2.0.0 each one needs a major version.

**Verdict:** 5 to decide before 2.0.0, 7 worth doing, 5 smaller ones. None is a bug in what the code does. They are about what callers can write, and what you'll have to keep. Items 1, 2, 4 and 5 are #146, and items 6 to 12 are #147. Item 3 is by design.

## Decide before 2.0.0

### 1. One-parameter `Audit` lambdas don't compile

*Reproduced.* `result.Audit(x => Console.WriteLine(x))` fails with CS0121, because the lambda fits `Action<Failure>`, `Action<T>` and `Action<Result<T>>` equally. The same goes for `maybe.Audit(x => …)` (`Action<T>` and `Action<Maybe<T>>`) and every `AuditAsync` equivalent. It only compiles when the lambda's body works for just one of the types, such as `x => Log(x.Message)`.

The one-callback overloads add nothing: `Audit(fOk)` is `IfOk(f)`, which is implemented as `Audit(@this, fOk: f)`. The same holds for `IfFailed`, `IfSome` and `IfNone`.

**Suggestion:** remove `Audit(Action<T>)`, `Audit(Action<Failure>)` and `Audit(Action)` and their async versions, and keep `Audit(Action<Maybe<T>>)`/`Audit(Action<Result<T>>)` and the two-callback forms. Callers move to `IfOk`/`IfFailed`/`IfSome`/`IfNone`, which already exist. The rename of the remaining `either` parameter (the whole value, not an `Either`) fits here too. Present since 1.x.

### 2. `Wrap.Logging.LogLevel` clashes with `Microsoft.Extensions.Logging.LogLevel`

*Reproduced.* Any file with `using Microsoft.Extensions.Logging;` and `using Wrap.Logging;`, which is normal in an ASP.NET Core app that logs failures, gets CS0104 on `LogLevel`. The two enums also differ:

| Wrap | Microsoft.Extensions.Logging |
|---|---|
| `Unknown` = 0 | `Trace` = 0 |
| `Verbose` = 1 | `Debug` = 1 |
| `Debug` = 2 | `Information` = 2 |
| `Information` = 4 | `Warning` = 3 |
| `Warning` = 8 | `Error` = 4 |
| `Error` = 16 | `Critical` = 5 |
| `Fatal` = 32 | `None` = 6 |

So casting one to the other silently gives the wrong level. Wrap's values are powers of two, but the enum isn't `[Flags]`, and nothing combines them.

`Microsoft.Extensions.Logging.Abstractions` is part of the .NET 11 shared framework, so using its `LogLevel` adds no package dependency to `Wrap.Common`.

**Suggestion:** use `Microsoft.Extensions.Logging.LogLevel` for `Failure.Level` and `[Failure(…, LogLevel.Error)]`, and remove `Wrap.Logging.LogLevel`. Alternatively, keep a Wrap enum under a name that can't clash (for example `FailureLevel`) with the same values as Microsoft's. The generator emits `(global::Wrap.Logging.LogLevel){x}`, so it changes too. Present since 1.x.

### 3. `x.Wrap()` makes a `Monad<T>`, while `M.Wrap` and `R.Wrap` make a `Maybe` and a `Result`

*Reproduced.* `ObjectExtensions.Wrap<T>(this T)` is an extension on every type, in `Wrap.Extensions`, which most callers import. `"hello".Wrap()` returns `Monad<string>`, so `.Wrap()` on any value in IntelliSense means something different from `M.Wrap(value)` and `R.Wrap(value)`, and `Monad<T>` has none of the `Map`/`Bind` methods. Nothing in `src`, `apps` or the README uses it, and `Monad<T>` already converts to `Maybe<T>` and `Result<T>` implicitly. The `IEnumerable<T>`, `WrapAsync` and `Task` versions behave the same way.

**Suggestion:** remove `ObjectExtensions`. If it's needed, rename it to `ToMonad()`. Present since 1.x.

**Decision: by design.** `x.Wrap()` is useful in consuming projects because `Monad<T>` converts implicitly to `Maybe<T>` and `Result<T>`, so `Result<User> r = user.Wrap();` works without naming either type. It stays as it is.

### 4. `IUintId` is cased differently from everything else

The interfaces are `IUintId` and `IUintId<TId>`. Their class is `UIntId<TId>`, and the neighbours are `IULongId`, `ULongId<TId>`, `UInt32Extensions` and `IdGenerator.UIntId<TId>()`.

**Suggestion:** rename them to `IUIntId` and `IUIntId<TId>`. This is a one-line fix now and a breaking change later. Present since 1.x.

### 5. `Either<T, T>` can't be assigned from a value

*Reproduced.* `Either<string, string> e = "x";` fails with CS0457, because the two implicit conversions, from `TLeft` and from `TRight`, are the same conversion. An `Either` whose two sides are the same type, or one derives from the other, is a normal way to say "ok text or error text".

**Suggestion:** document this on the operators, and point to `E.WrapLeft`/`E.WrapRight` or `new Left<T>(…)`/`new Right<T>(…)`, which the union conversions already accept. Removing the operators would fix it, but would also lose `Either<string, int> e = 5;` everywhere else, so a doc remark is the proportionate fix.

**Correction (#146):** the operators already said the conversion is ambiguous when both types are the same, which this review missed. The remark now also covers two cases it didn't. A value that converts equally well to both sides (a class implementing both of two interfaces) is ambiguous too. And when one side is a better match the compiler picks it without warning: with `Either<object, string>`, a `string` becomes a `Right`. Both were checked in a consumer project.

## Worth doing

### 6. Two `Match` APIs with different overload sets

`M.Match`/`M.MatchAsync`, `R.Match`/`R.MatchAsync` and `E.Match`/`E.MatchAsync` duplicate the `Match` extension methods, but not exactly:

| | static | extension |
|---|---|---|
| Maybe `MatchAsync` returning `Task` | 7 | 4 |
| Result `MatchAsync` returning `Task` | 7 | 2 |
| Either `MatchAsync` returning `Task` | 4 | 2 |

So whether `await task.MatchAsync(() => …, x => …)` compiles with `Action` callbacks depends on which form you use. With union types, `switch` is now the main way to match, so both forms are shorthand.

**Suggestion:** keep one set. Make the static `Match` functions internal, since the extensions call them, or give both forms the same overloads.

### 7. The `…If…Async` families have every sync/async combination

`MatchIfAsync` has 30 overloads on Maybe and 30 on Result. That's every combination of sync or async for `this`, `fNone`/`fFail`, `fTest`, `fTrue` and `fFalse`, except the ones that are `MatchIf` itself. The two-branch `IfAsync` has 7 per type, and the `MatchAsync` extensions 11. The common shapes resolve without ambiguity (checked: `MatchAsync(() => 0, async x => x)`, `IfAsync(…, async x => …, x => …)`, `MatchIfAsync(() => 0, x => x > 0, async x => x, x => -x)`). But the sets fill IntelliSense, and each new combinator has to repeat the pattern.

**Suggestion:** this is a judgement call, not a defect. One option is to keep only the all-sync form, the all-async form and the `Task<…>` receiver forms, since a sync function can always be passed as an async lambda. The cost is an async state machine per call for callers who mix. If you keep the full sets, consider documenting the rule once, so contributors keep them consistent.

### 8. Implementation helpers are public

`Wrap.Common` exposes `F.GetMonadTypes`, `F.GetMonadValueType`, `F.IsNullableValueType`, `F.Bind<TMonad, TValue>` and `F.Wrap<TMonad, TValue>`. Outside Common they are only used by the JSON converters, the MVC binders and the `WrapId` extensions. `Monad<TMonad, TValue>.Bind` duplicates `F.Bind`, and users already have `UserId.Wrap(5)` and `5.WrapId<UserId>()`. Elsewhere, `IdTypeHandler<,>.DefaultOrThrow` is public, and so are `WrapModelBinder<T>` and `IWrapModelBinder<T>.GetValue` (returning a `ValueTuple`). All of these are probably used only by Wrap itself.

**Suggestion:** make them internal, with `InternalsVisibleTo` for `Wrap` and `Wrap.Mvc`, which the repo already uses between projects. Keep `F.Format` and `F.DefaultCulture` public. Anything left public after 2.0 has to stay.

### 9. Not every exception derives from `WrapException`

`UnsafeUnwrapException`, `UnsafeUnwrapSingleException<TRight, TSingle>` and `ModelBinderException` derive from `Exception`. The others (`NullMonadValueException`, `NullMaybeValueException`, `JsonConverterException`) derive from `WrapException`, so `catch (WrapException)` misses the first three. `UnsafeUnwrapSingleException` is also generic, so it can only be caught by naming both type arguments.

**Suggestion:** derive all of them from `WrapException`, and make `UnsafeUnwrapSingleException` non-generic, with the types in its message.

### 10. `FailureKindJsonConverter` is in `Wrap`, the other converters in `Wrap.Json`

#144 made it public, so its namespace is now API. It lives in `Wrap.Result`, but the namespace doesn't have to follow the assembly.

**Suggestion:** move it to `Wrap.Json` before it's released. It hasn't shipped in a release yet, so this costs nothing now.

### 11. Class names that clash between namespaces

*Reproduced.* With `using Wrap.Extensions;` and `using Wrap.Linq;`, `ResultExtensions.Map(…)` called by its class name gives CS0104, because both namespaces have a `ResultExtensions`. Extension-method calls are unaffected. The same pattern appears in several places:

- `Wrap.Linq` has `MaybeToMaybeExtensions` and `MaybeToResultExtensions`, but `ResultExtensions` rather than `ResultToResultExtensions`.
- `Wrap.Testing` has `MaybeExtensions`, `ResultExtensions` and `EitherExtensions`, the same names as in `Wrap.Extensions`.
- `Wrap.Json.ListExtensions` and `Wrap.Mvc.ListExtensions` share a name.

**Suggestion:** rename `Wrap.Linq.ResultExtensions` to `ResultToResultExtensions`. Optionally, rename the Testing classes (for example `MaybeAssertions`) and the two `ListExtensions` (`JsonConverterListExtensions`, `ModelBinderProviderListExtensions`). These names only matter for static calls and IntelliSense, so this is low cost either way.

### 12. Abbreviated method names on `Failure`

The `Failure` methods are `Msg`, `Arg`, `Ctx`, `Lvl` and `Knd`. The .NET naming guidelines avoid abbreviations. The full words are taken, because `Failure` has properties called `Message`, `Args`, `Context`, `Level` and `Kind`, and the usual pattern for this is `WithMessage`, `WithArgs`, `WithContext`, `WithLevel` and `WithKind`. `Msg`, `Arg`, `Ctx` and `Lvl` come from 1.x, so users know them. `Knd` is new in beta.5.

**Suggestion:** your call. If you keep the short names, nothing is broken. If you change them, 2.0 is the only cheap time. At the least, decide `Knd` deliberately, since it's the one with no 1.x users.

## Smaller

### 13. Parameter names that don't match

Renaming a parameter only breaks callers who use named arguments, so these are cheap now:

- `Result.Unwrap(ifFailed, getValue)` vs `UnwrapAsync(ifFailed, returnThis)`.
- `Maybe.Unwrap(getValue)` vs `UnwrapAsync(fNone)`.
- `ToResult(noneHandler)` vs `ToMaybe(fFailureHandler)`.
- `ExceptionHandler e` in `R.Try` and `Map`, and `MemoryCacheEntryOptions opt` in `IWrapCache`.
- `Ctx(string class, string function)` and `ToResult(string class, string function)`, where `class` is a keyword, so callers write `@class:`.

### 14. Sync and async sets don't match

Some examples:

- `ToMaybe` (sync) takes only `Action<Failure>`. `ToMaybeAsync` also takes `Func<Failure, Maybe<T>>`, `Func<Failure, Task>` and `Func<Failure, Task<Maybe<T>>>`.
- `ToResultAsync` exists only on `Task<Maybe<T>>`. There's no `ToResultAsync(this Maybe<T>, Func<Task<Result<T>>>)`.
- `maybe.Unwrap(Func<None, T>)` exists, but `None` carries nothing, so it adds nothing over `Unwrap(Func<T>)`.

**Suggestion:** fill or trim the sets, so "every method has an async twin with the same callbacks" holds.

### 15. `GetSingle` has two styles

`result.GetSingle<List<int>, int>()` needs both type arguments, and `result.GetSingle(x => x.Value<int>())` goes through a public nested class, `ResultExtensions.FluentGetSingle<T>`. The two exist because `Result<List<int>>` isn't a `Result<IEnumerable<int>>`. They are a fair workaround, but two shapes for one operation is more to keep.

**Suggestion:** keep one of them. Optionally, add a `GetSingle<T>(this Result<IEnumerable<T>>)` for the common case.

### 16. `R.Ok()` returns `Result<bool>`

`R.Ok()` gives `R.True`. It's a success with no value, modelled as `true`, so `Result<bool>` means either "a yes/no answer" or "it worked", depending on the method. Many libraries use a `Unit` type for the second meaning.

**Suggestion:** your call. It's only worth changing if the double meaning has confused anyone.

### 17. Interfaces with `init` accessors

`IMonad<T>.Value`, `IWithException.Exception` and `IWithId<TId, TValue>.Id` are `{ get; init; }`. Every implementation must then have an `init` accessor, though nothing can call it through the interface.

**Suggestion:** make them `{ get; }`. This doesn't affect the existing implementations, which keep their `init` accessors.

## Left out

- `If` and `BindIf` look alike but differ when the test is false: `If` returns the original value, while `BindIf` returns `None`. The types force this, because `BindIf` can change `T`, so the docs are the fix. Check that they say it.
- `Maybe<T>` has `GetEnumerator` (`foreach` over a Maybe), but `Result<T>` and `Either` don't, except through `Unsafe()`. That's a design choice, not an inconsistency to fix.
- `FailureKind` exposes its well-known kinds as `static readonly` fields. Changing them to properties later is a binary break, but there's no reason to.
- The record members the compiler generates (`EqualityContract`, `<Clone>$`, `PrintMembers`) are public by design.

## Not checked

- The Caching and Dapper APIs were read, but not tried in a consumer.
- The MVC API was not tried in an app.
- Binary compatibility with beta.5, which only matters if anyone ships against the betas.
- Whether an API analyser (such as `Microsoft.CodeAnalysis.PublicApiAnalyzers`) is worth adding to track the API after 2.0.0. It would make every later change to the public API visible in review.
