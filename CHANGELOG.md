# Changelog

All notable changes to Wrap are listed here. The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

Entries up to 1.4.0 come from the GitHub release notes. 2.0.0 followed five betas (`2.0.0-beta.1` to `2.0.0-beta.5`, 13 to 26 November 2026), which are not listed separately: the 2.0.0 entry lists every change since 1.4.0. Its entries come from the commits, pull requests and [audit plan](https://github.com/bfren/wrap/blob/06396aaa58534d937fbe6eebcbaf78bd7006cd1f/docs/audit-plan.md).

## [Unreleased]

### Added

- A `[Failure]` method can take an `Exception` parameter (or a type derived from it, or a type parameter constrained to it). The failure is created by `R.Fail(ex)`, so it holds the exception and has its level (`Error`) and kind, unless the attribute sets them. The exception is left out of the message placeholders, and a `null` exception gives a plain message failure: `[Failure("Unable to read {Path}.")] static partial Failure UnreadableFile(Exception ex, string path);`. A method with more than one exception parameter is reported as `WRAP007`.

## [2.0.0-rc.1] - 2026-11-26

Wrap 2 is built on C# 15 union types. Wrap 1.x stays on a maintenance branch for .NET 8, 9, 10 and 11, and gets fixes only.

### Added

- The `[Failure]` source generator: put `[Failure("User {UserId} was not found")]` on a `static partial` method returning `Failure`, and the method body is written at compile time, with the context set to `Class.Method()`. It takes an optional `Kind`, for example `Kind = nameof(FailureKind.NotFound)`. It ships inside `Wrap.Result`, so installing `Wrap` is enough. It reports `WRAP001` to `WRAP006` for an invalid method, an empty message or kind, placeholders that don't match the parameters, and methods it can't implement (generic, `params`, `ref`/`out`/`in`, or in a `file` type).
- `FailureKind` and `Failure.Kind` say what sort of failure a `Failure` is, in terms that make sense outside any one transport: `Invalid`, `NotFound`, `Conflict`, `Unauthenticated`, `Forbidden`, `Unavailable`, `Timeout`, `Cancelled` or `Unexpected`. The default is `Unspecified`. The set is open: define your own kinds as static members, prefixed with your library or app name (for example `"Merlin.PayloadTooLarge"`). Set it with `Failure.WithKind()`, and check it in tests with `AssertKind()` in `Wrap.Result.Testing`.
- `Kind` is written to JSON as its name, or `null` when it is `Unspecified`. A kind that is missing, empty or only whitespace reads as `Unspecified`. `FailureKindJsonConverter` is public, in the `Wrap.Json` namespace, and `AddWrapConverters()` registers it, so a `Failure` also works with a System.Text.Json source-generated `JsonSerializerContext`.
- `Unsafe()` for `Either` types.
- `UnwrapSingle` works on `Result` and `Either` as well as `Maybe`, under `Unsafe()`.
- A typed ID can be a JSON dictionary key.
- `Maybe<enum>` binds in MVC, from a name (`Monday`, `monday`) or a number.
- `Html.TextBoxForMaybe(x => x.Property)` writes a text input for a `Maybe<T>` property: the value for `Some` (the inner value for a monad such as an ID), and empty for `None`, so it binds back to the same value. It takes optional HTML attributes.
- `Html.CheckBoxForMaybe(x => x.Property)` writes a checkbox for a `Maybe<bool>` property, ticked only for `Some(true)`. A checkbox always sends a value, so it binds back as `Some(true)` or `Some(false)`, never `None`.
- `LogLevel.ToMsLevel()` (in `Wrap.Extensions`) converts a Wrap log level to the matching `Microsoft.Extensions.Logging.LogLevel`. The two enums have different values, so always map with this and never cast.
- The `MatchAsync` extensions take every mix of sync and async callbacks on both a value and a `Task<…>` receiver, for `Maybe`, `Result` and `Either`. Before, the mixed forms with `Action` callbacks were only on the static `M`, `R` and `E` functions. `MatchIfAsync` gains the two forms it was missing.
- `result.ToMaybeAsync(async f => …)` works on a `Result<T>` as well as a `Task<Result<T>>`, for an async callback such as logging, and `maybe.ToResultAsync(async () => …)` works on a `Maybe<T>`.
- XML doc comments throughout the library.

### Changed

- **Breaking:** Wrap targets .NET 11 only.
- **Breaking:** `Maybe<T>`, `Result<T>` and `Either<TLeft, TRight>` are union structs, not reference types. `Some<T>`, `Ok<T>` and `Failure` are public cases you can pattern match on, and a `switch` over them is checked for exhaustiveness. `default(Maybe<T>)` is `None`. Code that checks `maybe is null`, uses `Maybe<T>?` or relies on inheritance must change.
- **Breaking:** `IEither.Match` is replaced by `TryGetLeft` and `TryGetRight`, and `IEither<TEither, TLeft, TRight>` is removed.
- **Breaking:** the `Failure` methods are renamed to follow the .NET naming guidelines: `Msg` is `WithMessage`, `Arg` is `WithArgs`, `Ctx` is `WithContext` (both overloads) and `Lvl` is `WithLevel`.
- **Breaking:** failures created from an exception, including by `R.Try` and `R.Fail(ex)`, set `Kind`: `Timeout` for a `TimeoutException` or a cancellation caused by one, `Cancelled` for any other `OperationCanceledException`, and `Unexpected` otherwise. An `AggregateException` that holds exactly one exception gets the kind of that exception, so a timeout reached through `task.Result` or `task.Wait()` is `Timeout`; one holding several exceptions is `Unexpected`. `Exception` still holds the original exception. `Kind` is part of equality, so these failures no longer equal one built by hand without a kind.
- **Breaking:** `M.Match`, `R.Match`, `E.Match` and their `MatchAsync` functions are internal. Call the extensions instead, such as `maybe.Match(…)` or `await task.MatchAsync(…)`.
- **Breaking:** `MatchIf` and `MatchIfAsync` take `fTrue` before `fFalse`, the same as `If`. Callers passing arguments by position must swap them.
- **Breaking:** `IUintId` and `IUintId<TId>` are renamed `IUIntId` and `IUIntId<TId>`, to match `UIntId<TId>` and `IULongId`.
- **Breaking:** `Wrap.Linq.ResultExtensions` is renamed `ResultToResultExtensions`, so it no longer clashes with `Wrap.Extensions.ResultExtensions` when both namespaces are imported. The assertion classes in `Wrap.Testing` are renamed too: `MaybeAssertionExtensions`, `ResultAssertionExtensions`, `FailureAssertionExtensions` and `EitherAssertionExtensions`.
- **Breaking:** every exception derives from `WrapException`, so `catch (WrapException)` catches them all. This includes `ModelBinderException` and the new `UnsafeUnwrapException` and `UnsafeUnwrapSingleException`.
- **Breaking:** `UnwrapSingle` is under `Unsafe()` and throws `UnsafeUnwrapSingleException`, which names the two types, when the list doesn't have exactly one item.
- **Breaking:** `F.GetMonadTypes`, `F.GetMonadValueType`, `F.IsNullableValueType` and `F.Wrap` are internal, and `IdTypeHandler.DefaultOrThrow` is `private protected`. They were implementation helpers.
- **Breaking:** `WrapCache` `GetValue`, `GetOrCreate` and `GetOrCreateAsync` return `Result<TValue>`, and `LastFailure` is removed, because every caller shared it. Value factories return `T` or `Result<T>` instead of `Maybe<T>`, and a `Failure` from a factory reaches the caller and is not cached. There is no locking any more: a factory can run more than once when calls for a missing key arrive together.
- **Breaking:** invalid JSON for a `Maybe<T>` or a typed ID throws `JsonException`, instead of reading as `0` or `Guid.Empty`. JSON `null` is still `None`.
- **Breaking:** an MVC value that is present but doesn't parse fails binding with a ModelState error (a 400 with `[ApiController]`), instead of binding as `None`. Missing or empty values still bind as `None`.
- **Breaking:** `ToResult` no longer catches exceptions thrown by its `fNone` handler, or from awaiting a faulted task, so they reach the caller the same way as in `Bind`.
- **Breaking:** `Bind` and `Map` on lists of `Maybe<T>` run on every item. `FilterBind` and `FilterMap` still run only on `Some<T>` items.
- **Breaking:** values are written using `F.DefaultCulture` (en-GB unless you set it), not the current culture, in `ToString()` of `Maybe`, `Result`, `Either`, `Some`, `Ok`, `Left`, `Right` and `Monad`, in `Html.HiddenForMonad`, in `F.Format` with a single value, and in the `Unsafe().Unwrap()` and Dapper overflow exception messages. So `M.Wrap(1.5m).ToString()` is `1.5` on a French server, not `1,5`.
- **Breaking:** Dapper ID handlers throw `OverflowException` for a number stored as text that doesn't fit the ID type, such as `"3000000000"` for an `IntId` or `"5.5"`. Before, it silently became the default ID (0 or `Guid.Empty`). A whole number stored as text is read as that number, so `"42.0"`, `"1,000"` and `"4.2E1"` give an integer ID of 42, 1000 and 42. Text that isn't a number still reads as the default ID. The handlers also read whole `decimal` values (`5.0m`), and the Guid handler reads 16-byte arrays.
- Parameter names match across each sync and async family. This only breaks calls that use named arguments: `Unwrap` and `UnwrapAsync` take `fNone` on a `Maybe`, and `fFail` and `fValue` on a `Result`, where they took `getValue`, `ifFailed` and `returnThis`. `Unwrap` on an `Either` takes `fLeft` (was `getValue`). `ToResult` takes `fNone` (was `noneHandler`), `ToMaybe` takes `fFail` (was `fFailureHandler`), `R.Try` and `Map` take `handler` (was `e`), `IWrapCache` takes `options` (was `opt`), `WithContext` and `ToResult` take `className` and `functionName` (was `@class` and `function`), and the `Audit` and `AuditAsync` callback that receives the whole value is `f` (was `either`).
- `F.Format`, and so `Failure.ToString()`, never throws: literal braces are kept, `{{name}}` is literal text, and unsupported input returns the format string unchanged. A format with a single value, such as `F.Format("{0:0.00}", 1.5m)`, now applies the format, which it ignored before, so an invalid one, such as `F.Format("Total {0:Q}", 1.5m)`, returns the format string unchanged, as it already did for several values.
- `M.ParseDateTime` keeps UTC times as UTC, instead of converting them to server local time. `M.ParseDateTimeOffset` reads input with no offset as UTC.
- `Failure.Exception` is left out of JSON, so a failed `Result` holding an exception can be written, and stack traces stay out of responses.
- `Some` and `Ok` values no longer allocate, and `Result<T>` stays small by keeping its failure in a box.
- `Wrap.Id.Testing` depends on `Rnd` 11.0.0.
- For contributors only, with no change to the packages: the build is now the only style check. `AnalysisLevel` is pinned to `11.0-recommended`, so a newer SDK can't add rules that fail CI, and CI fails on any warning except low-severity package vulnerabilities (NU1901). CI no longer runs `dotnet format`. Run `dotnet build -warnaserror` to see what CI will reject. `.editorconfig` now only holds settings that differ from the .NET defaults, and most style preferences are suggestions.

### Removed

- **Breaking:** failure logging: `F.Log`, the logging hooks in `F`, the logging in `Audit`, and `R.Pass`. A `Failure` still carries a `LogLevel` for you to log yourself, and `LogLevel.ToMsLevel()` maps it.
- **Breaking:** the implicit conversions from `TLeft` and `TRight` to `Either<TLeft, TRight>`. They were ambiguous when both types matched a value, and otherwise could pick a side without warning. Use `new Left<T>(…)`, `new Right<T>(…)`, `E.WrapLeft` or `E.WrapRight`.
- **Breaking:** the one-callback `Audit` and `AuditAsync` overloads, such as `result.Audit(x => …)` or `maybe.Audit(fSome)`. A lambda fitted several of them, so the call didn't compile (CS0121). Use `IfOk`, `IfFailed`, `IfSome` or `IfNone`, the two-callback form, or the form that takes the whole value.
- **Breaking:** the `Select` methods that turned a `Maybe` into a `Result`. They made `from x in maybe select …` ambiguous (CS1940). Use `ToResult` then `Map`.
- **Breaking:** `ContinueIf`. Use `Filter`.
- **Breaking:** `F.Bind` and `Monad<TMonad, TValue>.Bind`, which only repeated `Wrap`.
- **Breaking:** `maybe.Unwrap(Func<None, T>)`, which added nothing over `Unwrap(Func<T>)` because `None` carries no value.
- **Breaking:** `result.GetSingle<TList, TSingle>()` and `result.GetSingle<TList, TSingle>(onError)`. Use `result.GetSingle(x => x.Value<TSingle>())`, which infers the types and checks at compile time that the list holds `TSingle` items. `FluentGetSingle<T>` can no longer be created directly: its constructor is internal.
- **Breaking:** the `FailureGenerator` and `NoneGenerator` testing helpers. Use `R.Fail(…)` and `M.None` or `default(Maybe<T>)`.
- The empty public `WrapCache` base class.

### Fixed

- `GetOrCreate` failed on every call when `IMemoryCache` had a `SizeLimit`.
- `F.Format` used the first argument twice in strings that mixed numbered and named placeholders, and could print the argument list's own `Length` for a `null` argument.
- `Wrap.Id.Testing` threw `FileNotFoundException` because the `Rnd` dependency was left out of the package.
- `new Monad<string>().ToString()` threw when the value wasn't set, and `HiddenForMonad` threw when the ID was `null`.

## [1.4.0] - 2026-11-11

### Added

- .NET 11 as a target framework.

### Changed

- Updated packages: `Microsoft.Extensions.*` 11.0.0 for .NET 10 and later (.NET 8 and 9 stay on 10.0.14).

## [1.3.0] - 2026-11-07

### Added

- `Then()` extension for `Task<T>`.

### Changed

- **Breaking:** `Maybe<T>` `None` is written to JSON as `null`, and `null` reads back as `None`.
- **Breaking:** Dapper ID handlers throw `OverflowException` when a number doesn't fit the ID type, and send `DBNull` for null IDs.
- **Breaking:** async `IEnumerable` extension methods return `Task<IEnumerable<T>>` instead of `Task<List<T>>`.
- **Breaking:** `MapIfAsync` returns a `Failure` when the map function throws.
- **Breaking:** `ElementAtOrNone` returns `None` for a negative index.
- Failures are logged once when passed through `Bind` and `Map`, and compared by value only.
- `FirstOrNone`, `LastOrNone`, `ElementAtOrNone` and `SingleOrNone` read arrays and lists by index.
- `HiddenForMonad` avoids compiling an expression for simple properties.
- Updated packages.

### Fixed

- A race in `WrapCache` failures: it now uses per-key locks and allows nested `GetOrCreate` calls.
- The MVC model binder escapes input before parsing it as JSON.

## [1.2.10] - 2026-10-28

### Changed

- Updated packages.

## [1.2.9] - 2026-10-23

### Changed

- Updated packages.

## [1.2.8] - 2026-09-16

### Changed

- Updated packages.

## [1.2.7] - 2026-08-14

### Changed

- Updated packages.

## [1.2.6] - 2026-07-03

### Changed

- Updated packages.

## [1.2.5] - 2026-05-16

### Changed

- Updated packages.

## [1.2.4] - 2026-04-22

### Changed

- Updated packages.

## [1.2.3] - 2026-03-13

### Changed

- Updated packages.

### Fixed

- The assert message when args is empty.

## [1.2.2] - 2026-03-06

### Changed

- Performance improvements.
- Updated packages.

## [1.2.1] - 2026-02-20

### Changed

- Improved test coverage, and the NuGet packages are attached to each release.

## [1.2.0] - 2026-02-17

### Changed

- **Breaking:** `AddWrapConverters` is an extension of `IList<JsonConverter>`.
- A generic `Monad<T>` model binder provider.
- Explicit implementation of `IWithId.Id`.
- Updated packages.

## [1.1.0] - 2026-02-08

### Changed

- **Breaking:** the JSON implementation is brought together in the `Wrap` (All) package.
- **Breaking:** major updates to the `IEnumerable` extension methods.
- **Breaking:** numeric ID values are written to JSON as numbers.
- **Breaking:** `IUnion` and `Union` are renamed `IMonad` and `Monad`.
- **Breaking:** renamed function arguments for the `If` extensions.
- **Breaking:** improved ID interfaces for use in entities.
- LINQ queries can contain both `Maybe<T>` and `Result<T>`.
- Improved object `Wrap` and the performance of `F.Format`.

### Fixed

- `F.Format` when passed a single value.

## [1.0.0] - 2026-01-27

First release. Wrap C# values in monads: `Maybe<T>` (`Some<T>` or `None`), `Result<T>` (`Ok<T>` or `Failure`) and `Id<T>` strong IDs, composed with `Map`, `Bind` and LINQ query syntax. It followed 28 beta releases, from `1.0.0-beta.23092207` (22 September 2023) to `1.0.0-beta.26012610` (27 January 2026), which had no release notes.

[Unreleased]: https://github.com/bfren/wrap/compare/v2.0.0-rc.1...HEAD
[2.0.0-rc.1]: https://github.com/bfren/wrap/compare/v1.4.0...v2.0.0-rc.1
[1.4.0]: https://github.com/bfren/wrap/compare/v1.3.0...v1.4.0
[1.3.0]: https://github.com/bfren/wrap/compare/v1.2.10...v1.3.0
[1.2.10]: https://github.com/bfren/wrap/compare/v1.2.9...v1.2.10
[1.2.9]: https://github.com/bfren/wrap/compare/v1.2.8...v1.2.9
[1.2.8]: https://github.com/bfren/wrap/compare/v1.2.7...v1.2.8
[1.2.7]: https://github.com/bfren/wrap/compare/v1.2.6...v1.2.7
[1.2.6]: https://github.com/bfren/wrap/compare/v1.2.5...v1.2.6
[1.2.5]: https://github.com/bfren/wrap/compare/v1.2.4...v1.2.5
[1.2.4]: https://github.com/bfren/wrap/compare/v1.2.3...v1.2.4
[1.2.3]: https://github.com/bfren/wrap/compare/v1.2.2...v1.2.3
[1.2.2]: https://github.com/bfren/wrap/compare/v1.2.1...v1.2.2
[1.2.1]: https://github.com/bfren/wrap/compare/v1.2.0...v1.2.1
[1.2.0]: https://github.com/bfren/wrap/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/bfren/wrap/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/bfren/wrap/releases/tag/v1.0.0
