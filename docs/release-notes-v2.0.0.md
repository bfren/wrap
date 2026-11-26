Wrap 2 is built on C# 15 union types and targets .NET 11 only. Wrap 1.x stays on a maintenance branch for .NET 8, 9, 10 and 11, and gets fixes only.

These notes cover every change since 1.4.0, including the five 2.0.0 betas. The [CHANGELOG](https://github.com/bfren/wrap/blob/main/CHANGELOG.md) has the detail and what to use instead of each removed member.

## Breaking changes
* Targeting .NET 11 only
* Making `Maybe<T>`, `Result<T>` and `Either<TLeft, TRight>` union structs, with `Some<T>`, `Ok<T>` and `Failure` as public cases checked for exhaustiveness in a `switch`; `default(Maybe<T>)` is `None`, so `is null` checks, `Maybe<T>?` and inheritance must change
* Replacing `IEither.Match` with `TryGetLeft` and `TryGetRight`, and removing `IEither<TEither, TLeft, TRight>`
* Renaming the `Failure` methods: `Msg`, `Arg`, `Ctx` and `Lvl` are `WithMessage`, `WithArgs`, `WithContext` and `WithLevel`
* Setting `Failure.Kind` on failures created from an exception (`Timeout`, `Cancelled` or `Unexpected`, looking inside an `AggregateException` with one exception); `Kind` is part of equality
* Making the static `M.Match`, `R.Match` and `E.Match` functions internal: use the extensions
* Taking `fTrue` before `fFalse` in `MatchIf` and `MatchIfAsync`, the same as `If`
* Renaming `IUintId` to `IUIntId`, `Wrap.Linq.ResultExtensions` to `ResultToResultExtensions`, and the `Wrap.Testing` assertion classes
* Deriving every exception from `WrapException`
* Moving `UnwrapSingle` under `Unsafe()`, throwing `UnsafeUnwrapSingleException` when the list doesn't have exactly one item
* Making `F.GetMonadTypes`, `F.GetMonadValueType`, `F.IsNullableValueType`, `F.Wrap` and `IdTypeHandler.DefaultOrThrow` non-public
* Returning `Result<TValue>` from `WrapCache`, removing `LastFailure` and the per-key locks, and not caching a failure from a value factory
* Throwing `JsonException` for invalid `Maybe<T>` or typed ID JSON, instead of reading `0` or `Guid.Empty`
* Failing MVC binding with a ModelState error for a value that doesn't parse, instead of binding `None`
* Letting exceptions from the `ToResult` handler reach the caller
* Running `Bind` and `Map` on every item in lists of `Maybe<T>`
* Writing values with `F.DefaultCulture` instead of the current culture in `ToString()`, `HiddenForMonad`, `F.Format` and exception messages
* Throwing `OverflowException` in Dapper ID handlers for text that is a number but doesn't fit the ID type
* Matching parameter names across each sync and async family (breaks named arguments only)
* Removing failure logging (`F.Log`, the logging hooks, logging in `Audit`, and `R.Pass`)
* Removing the implicit conversions from `TLeft` and `TRight` to `Either<TLeft, TRight>`
* Removing the one-callback `Audit` overloads, the `Maybe` to `Result` `Select` methods, `ContinueIf`, `F.Bind`, `Monad.Bind`, `Unwrap(Func<None, T>)`, `GetSingle<TList, TSingle>()`, and the `FailureGenerator` and `NoneGenerator` testing helpers

## Major changes
* Adding the `[Failure]` source generator, which writes `Failure` methods from a message template at compile time, with diagnostics `WRAP001` to `WRAP006`
* Adding `FailureKind` and `Failure.Kind`, an open set of transport-neutral failure kinds, with `WithKind()`, `AssertKind()` and JSON support
* Adding `Unsafe()` for `Either`, and `UnwrapSingle` for `Result` and `Either`
* Adding every mix of sync and async callbacks to the `MatchAsync` extensions on `Maybe`, `Result` and `Either`
* Adding `Html.TextBoxForMaybe` and `Html.CheckBoxForMaybe` for MVC forms
* Binding `Maybe<enum>` in MVC from a name or a number
* Allowing typed IDs as JSON dictionary keys
* Removing allocation from `Some` and `Ok` values

## Minor changes
* Adding `LogLevel.ToMsLevel()` to map to `Microsoft.Extensions.Logging.LogLevel`
* Adding `ToMaybeAsync` on `Result<T>` and `ToResultAsync` on `Maybe<T>`
* Making `FailureKindJsonConverter` public and registering it in `AddWrapConverters()`
* Making `F.Format` and `Failure.ToString()` never throw, and applying the format with a single value
* Keeping UTC times as UTC in `M.ParseDateTime`, and reading input with no offset as UTC in `M.ParseDateTimeOffset`
* Leaving `Failure.Exception` out of JSON
* Reading whole `decimal` values and 16-byte Guid arrays in the Dapper ID handlers
* Fixing `GetOrCreate` when `IMemoryCache` has a `SizeLimit`
* Fixing `F.Format` with mixed numbered and named placeholders, and with a `null` argument
* Fixing `FileNotFoundException` in `Wrap.Id.Testing` by packaging the `Rnd` dependency, now 11.0.0
* Fixing `ToString()` on an unset `Monad<string>` and `HiddenForMonad` with a `null` ID
* Adding XML doc comments throughout the library

## Development updates
* Making the build the only style check, pinning `AnalysisLevel` to `11.0-recommended` and failing CI on any warning except NU1901
* Trimming `.editorconfig` to the settings that differ from the .NET defaults


**Full Changelog**: https://github.com/bfren/wrap/compare/v1.4.0...v2.0.0-rc.1
