# Changelog

All notable changes to Wrap are listed here. The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

Entries up to 1.4.0 come from the GitHub release notes. The 2.0.0 betas had no release notes, so their entries come from the commits, pull requests and [audit plan](docs/audit-plan.md).

## [Unreleased]

### Changed

- **Breaking:** a failure created from an `AggregateException` that holds exactly one exception gets the kind of that exception. So a timeout reached through `task.Result` or `task.Wait()` is `Timeout`, not `Unexpected`. An aggregate holding several exceptions is still `Unexpected`. `Exception` still holds the `AggregateException`. `Kind` is part of equality, so these failures no longer equal one built with `Unexpected`.

### Fixed

- A `Failure` can be written and read with a System.Text.Json source-generated `JsonSerializerContext`. Before, it built with warning SYSLIB1220 and threw `NotSupportedException`, because `FailureKindJsonConverter` was internal. It is now public.
- `[Failure]` methods compile in a project with a namespace ending in `.Wrap`, such as `MyApp.Wrap`. The generated code now uses `global::` type names.
- A JSON kind that is empty or only whitespace reads as `FailureKind.Unspecified`. Before, it gave a kind that printed the same as `Unspecified` but wasn't equal to it. A kind with such a name is also written as `null`, so it reads back the same way.
- For contributors only: CI no longer fails on low-severity package vulnerabilities (NU1901), and `AnalysisLevel` is pinned to `11.0-recommended` so a newer SDK can't add rules that fail CI.

## [2.0.0-beta.5] - 2026-11-26

### Added

- `FailureKind` and `Failure.Kind` say what sort of failure a `Failure` is, in terms that make sense outside any one transport: `Invalid`, `NotFound`, `Conflict`, `Unauthenticated`, `Forbidden`, `Unavailable`, `Timeout`, `Cancelled` or `Unexpected`. The default is `Unspecified`. The set is open: define your own kinds as static members, prefixed with your library or app name (for example `"Merlin.PayloadTooLarge"`).
- `Failure.Knd()` sets the kind, alongside `Msg()`, `Arg()`, `Ctx()` and `Lvl()`.
- `[Failure]` takes a `Kind` named argument, for example `Kind = nameof(FailureKind.NotFound)`. A kind that is set but empty is reported as `WRAP006`.
- `AssertKind()` in `Wrap.Result.Testing`.

### Changed

- **Breaking:** failures created from an exception, including by `R.Try` and `R.Fail(ex)`, set `Kind`: `Timeout` for a `TimeoutException` or a cancellation caused by one, `Cancelled` for any other `OperationCanceledException`, and `Unexpected` otherwise. `Kind` is part of equality, so these no longer equal a failure built by hand without a kind.
- `Kind` is written to JSON as its name, or `null` when it is `Unspecified`.
- For contributors only, with no change to the packages: the build is now the only style check. It uses the `latest-recommended` analysers and fails CI on any warning, and CI no longer runs `dotnet format`. Run `dotnet build -warnaserror` to see what CI will reject. `.editorconfig` now only holds settings that differ from the .NET defaults, and most style preferences are suggestions.

## [2.0.0-beta.4] - 2026-11-18

### Added

- The `[Failure]` source generator: put `[Failure("User {UserId} was not found")]` on a `static partial` method returning `Failure`, and the method body is written at compile time, with the context set to `Class.Method()`. It ships inside `Wrap.Result`, so installing `Wrap` is enough. It reports `WRAP001` to `WRAP005` for an invalid method, an empty message, placeholders that don't match the parameters, and methods it can't implement (generic, `params`, `ref`/`out`/`in`, or in a `file` type).
- A typed ID can be a JSON dictionary key.
- `Maybe<enum>` binds in MVC, from a name (`Monday`, `monday`) or a number.
- `UnwrapSingle` works on `Maybe`, `Result` and `Either`, under `Unsafe()`.

### Changed

- **Breaking:** `WrapCache` `GetValue`, `GetOrCreate` and `GetOrCreateAsync` return `Result<TValue>`, and `LastFailure` is removed, because every caller shared it. Value factories return `T` or `Result<T>` instead of `Maybe<T>`, and a `Failure` from a factory reaches the caller and is not cached. There is no locking any more: a factory can run more than once when calls for a missing key arrive together.
- **Breaking:** invalid JSON for a `Maybe<T>` or a typed ID throws `JsonException`, instead of reading as `0` or `Guid.Empty`. JSON `null` is still `None`.
- **Breaking:** an MVC value that is present but doesn't parse fails binding with a ModelState error (a 400 with `[ApiController]`), instead of binding as `None`. Missing or empty values still bind as `None`.
- **Breaking:** `MatchIf` and `MatchIfAsync` take `fTrue` before `fFalse`, the same as `If`. Callers passing arguments by position must swap them.
- **Breaking:** `ToResult` no longer catches exceptions thrown by its `noneHandler`, or from awaiting a faulted task, so they reach the caller the same way as in `Bind`.
- **Breaking:** `Bind` and `Map` on lists of `Maybe<T>` run on every item. `FilterBind` and `FilterMap` still run only on `Some<T>` items.
- **Breaking:** `UnwrapSingle` moved under `Unsafe()` and throws `UnsafeUnwrapSingleException` when the list doesn't have exactly one item.
- `M.ParseDateTime` keeps UTC times as UTC, instead of converting them to server local time. `M.ParseDateTimeOffset` reads input with no offset as UTC.
- `F.Format`, and so `Failure.ToString()`, never throws: literal braces are kept, `{{name}}` is literal text, and unsupported input returns the format string unchanged.
- Dapper ID handlers read whole `decimal` values (`5.0m`), and the Guid handler reads 16-byte arrays.
- `Failure.Exception` is left out of JSON, so a failed `Result` holding an exception can be written, and stack traces stay out of responses.
- XML doc comments throughout the library.

### Removed

- **Breaking:** the `Select` methods that turned a `Maybe` into a `Result`. They made `from x in maybe select …` ambiguous (CS1940). Use `ToResult` then `Map`.
- **Breaking:** `ContinueIf`. Use `Filter`.
- **Breaking:** the `FailureGenerator` and `NoneGenerator` testing helpers. Use `R.Fail(…)` and `M.None` or `default(Maybe<T>)`.
- The empty public `WrapCache` base class.

### Fixed

- The `[Failure]` generator: overloads, generic containing types, interfaces, unions, the global namespace, a single array parameter, parameters named `message` or `context`, and names that are keywords (`@class`) now generate code that compiles. It no longer regenerates every file on every edit.
- `GetOrCreate` failed on every call when `IMemoryCache` had a `SizeLimit`.
- `F.Format` used the first argument twice in strings that mixed numbered and named placeholders, and could print the argument list's own `Length` for a `null` argument.
- `Wrap.Id.Testing` threw `FileNotFoundException` because the `Rnd` dependency was left out of the package.
- `new Monad<string>().ToString()` threw when the value wasn't set, and `HiddenForMonad` threw when the ID was `null`.

## [2.0.0-beta.3] - 2026-11-13

### Fixed

- Brought back the implicit conversions from a monad to `Result<T>` and `Maybe<T>`.

## [2.0.0-beta.1] - 2026-11-13

Wrap 2 is built on C# 15 union types. Wrap 1.x stays on a maintenance branch for .NET 8, 9, 10 and 11, and gets fixes only.

### Added

- `Unsafe()` for `Either` types.

### Changed

- **Breaking:** Wrap targets .NET 11 only.
- **Breaking:** `Maybe<T>`, `Result<T>` and `Either<TLeft, TRight>` are union structs, not reference types. `Some<T>`, `Ok<T>` and `Failure` are public cases you can pattern match on, and a `switch` over them is checked for exhaustiveness. `default(Maybe<T>)` is `None`. Code that checks `maybe is null`, uses `Maybe<T>?` or relies on inheritance must change.
- **Breaking:** `IEither.Match` is replaced by `TryGetLeft` and `TryGetRight`, and `IEither<TEither, TLeft, TRight>` is removed.
- `Some` and `Ok` values no longer allocate, and `Result<T>` stays small by keeping its failure in a box.

### Removed

- **Breaking:** failure logging: `F.Log`, the logging hooks in `F`, the logging in `Audit`, and `R.Pass`. A `Failure` still carries a `LogLevel` for you to log yourself.

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

[Unreleased]: https://github.com/bfren/wrap/compare/v2.0.0-beta.5...HEAD
[2.0.0-beta.5]: https://github.com/bfren/wrap/compare/v2.0.0-beta.4...v2.0.0-beta.5
[2.0.0-beta.4]: https://github.com/bfren/wrap/compare/v2.0.0-beta.3...v2.0.0-beta.4
[2.0.0-beta.3]: https://github.com/bfren/wrap/compare/v2.0.0-beta.1...v2.0.0-beta.3
[2.0.0-beta.1]: https://github.com/bfren/wrap/compare/v1.4.0...v2.0.0-beta.1
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
