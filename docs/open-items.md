# Open items and decisions

What is still to do for Wrap 2.0.0, and the decisions that explain why the code is the way it is. Everything else from the earlier plans is done.

This file replaces four documents, which are kept in the git history at [`06396aa`](https://github.com/bfren/wrap/tree/06396aaa58534d937fbe6eebcbaf78bd7006cd1f/docs):

- `union-types-analysis.md`: the move to C# 15 union types, with the benchmark reasoning in section 10.
- `audit-plan.md`: six audits for bugs, all fixed or decided.
- `api-review.md`: the public API review behind #146 and #147, both done.
- `style-checks-plan.md`: making the build the only style gate, done.

Last checked against the code on 26 November 2026.

## To do

### At release

- [ ] Write the wiki docs and the 1.x to 2.0 migration guide. They were held until the API settled.
- [ ] Say in the README that the Testing packages are for xunit v3 projects only. They reference `xunit.v3.assert` with `PrivateAssets="All"`.
- [ ] After 2.0.0 is published, set `EnablePackageValidation` back to `true` in `src/Directory.Build.props`. `PackageValidationBaselineVersion` is already `2.0.0`.

### After 2.0.0

- [ ] Decide whether to add `Microsoft.CodeAnalysis.PublicApiAnalyzers`, so every change to the public API shows in review.

## Never checked

- The cache under load.
- Packages packed from the repo, installed in a separate consumer project.
- The Testing packages inside a real xunit v3 test project.
- MVC binding with `[ApiController]`. It was checked in a scratch app (a bad value gives a 400), but there is no test for it in the repo.
- `Html.TextBoxForMaybe` and `Html.CheckBoxForMaybe` rendered in a real view. Their tests check the arguments passed to MVC's helpers.
- The shared workflows in `bfren/dotnet` (`prepare.yml`, `test.yml`, `publish.yml` and `notify.yml`).
- The vendored JavaScript under `apps/AppMvc/wwwroot`.
- Binary compatibility with the 2.0.0 betas, which only matters if anyone ships against them.

## Decisions

Each of these was looked at and kept on purpose. Change one only for a new reason.

### API shape

- **`x.Wrap()` returns a `Monad<T>`**, not a `Maybe<T>` or `Result<T>` like `M.Wrap` and `R.Wrap`. It converts implicitly to both, and projects that use Wrap rely on that (#146).
- **Every sync/async mix of the callbacks stays** in the `…Async` families. The rule and its exceptions are in CLAUDE.md (#147).
- **`Failure` stays a `readonly record struct`.** A record class would save one 40-byte copy but allocate a new object on every `WithMessage` or `WithContext` call, and make `default(Failure)` `null`.
- **`default(Result<T>)` is a `Failure`** with the message `Uninitialised Failure`. **`default(Either<TLeft, TRight>)` is `Left(default)`**, matching `default(Maybe<T>)` being `None`.
- **`Maybe<T>` has `GetEnumerator`**, so you can `foreach` over it. `Result<T>` and `Either` don't, except through `Unsafe()`.
- **`IEither` and `Unsafe` stay**, with `TryGetLeft` and `TryGetRight` in place of the old `IEither.Match`.
- **`Audit` ignores exceptions thrown by its callbacks**, so auditing never changes the result. Its docs say so.
- **`AssertMessage` doesn't check args when none are given**, so a test can check the message alone. Use `Assert.Empty(failure.Args)` to check there are none.
- **`GetSingle` has only the fluent form**, `result.GetSingle(x => x.Value<int>())`. It infers the list type, and checks at compile time that the list holds the item type. The explicit form, `GetSingle<List<int>, int>()`, is internal: it does the work behind the fluent one.
- **`R.Ok()` returns `Result<bool>`** holding `true`, so `Result<bool>` means either a yes/no answer or "it worked". A `Unit` type would add a type and break every caller.
- **`IMonad<T>.Value`, `IWithException.Exception` and `IWithId<TId, TValue>.Id` keep their `init` accessors.** Generic code sets them through the interface: `F.Wrap` creates monads with `new TMonad { Value = … }`, and a data layer can create entities with `new T { Id = … }`.
- **Callback parameters are named for the case they handle**: `fNone`, `fSome`, `fFail`, `fOk`, `fLeft` and `fRight`, with `fValue` for a fallback value. Sync and async forms of a method use the same names.
- **There is no sync `ToMaybe(Func<Failure, Maybe<T>>)`**, though `ToMaybeAsync` has one. A `T` converts to `Maybe<T>`, so on a `Result<string>` a logging lambda such as `f => Log(f)`, where `Log` returns a string, would pick it over `ToMaybe(Action<Failure>)` and return `Some` instead of `None`. `ToMaybe_Tests` holds this.

### Culture and formatting

- **`F.DefaultCulture` is one setting for the whole process**, en-GB by default. Wrap is a library, so the app sets it once at startup. Everything formats and parses with it (rule in CLAUDE.md).
- **`F.Format` looks up named placeholders on the static type `T`**, not the runtime type, so `F.Format("{Name}", (object)user)` leaves `{Name}` in the text. Nothing in Wrap calls it that way.
- **`F.Format` prints an array argument as its type name**, as `string.Format` does. Pass `string.Join(", ", names)` to show the values.
- **`F.Format` never throws.** Invalid input, including an invalid format such as `{0:Q}`, returns the format string unchanged.

### JSON

- **`None` is written as JSON `null`**, by Wrap's own converter, which takes priority over the built-in union handling (that would write `{}`). With `WhenWritingNull` a `None` is still written, because `Maybe<T>` is a struct. Use `WhenWritingDefault`, which skips `None` and still writes `Some(0)`.
- **In a nested `Maybe<Maybe<T>>`, `Some(None)` is written as `null`**, so it reads back as `None`.
- **A `Maybe<T>` can't be a dictionary key.** Typed IDs can.
- **The converters detect empty input by matching English exception text.** No clean fix was found: a reader passed in directly starts at `JsonTokenType.None` even when it holds valid JSON.

### MVC

- **A missing or empty `Maybe<T>` value binds as `None`, and a value that doesn't parse fails binding** (a 400 with `[ApiController]`). The attempted value for `None` is empty, so a form shown again leaves the field blank.
- **`Html.CheckBoxForMaybe` never gives `None`.** A checkbox always posts a value, so it binds as `Some(true)` or `Some(false)`. Use a select or radio buttons to keep `None`.

### Cache

- **`WrapCache` has no locking.** Calls for a missing key that arrive together each run their value factory, and the last value stored wins. A `shortcut:` comment in `WrapCache.cs` says when to add per-key de-duplication.
- **Options you pass must set `Size`.** Calls without options use `Size = 1`, which `MemoryCache` ignores when there is no size limit (#129).
- **The sync and async `GetOrCreate` bodies stay as near-copies.** Merging them would mean blocking on async code.

### Dapper

- **A database value that is a number but doesn't fit the ID type throws `OverflowException`**, whether it is a number type or text such as `"3000000000"`. A whole number stored as text (`"42.0"`, `"1,000"`) is read as that number. Text that isn't a number gives the default ID, as `Parse_Tests` expects.
- **A 16-byte array is read as a Guid in `Guid.ToByteArray()` order.** Bytes in RFC 4122 order (MySQL `UUID_TO_BIN`, Oracle `SYS_GUID`) read back as a different Guid. A `shortcut:` comment in `GuidIdTypeHandler.cs` records this.

### Release and CI

- **Wrap 2.x targets .NET 11 only.** 1.x lives on the `v1` branch and gets fixes only. Targeting .NET 10 as well was tried and not merged (see CLAUDE.md).
- **Allocation regressions are checked by the `Allocations` workflow**, not unit tests, because CI tests a Debug build, which boxes values that a Release build doesn't. The limits are in `apps/Benchmarks/allocations.json`.
- **The build is the only style gate**, so the shared workflow runs with `style: false` (see CLAUDE.md).
