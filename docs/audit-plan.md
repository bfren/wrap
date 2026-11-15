# Audit plan

Repo audit of branch `108-switch-to-use-union-types`, 12 November 2026. Updated 14 November 2026 after the fixes on #110 and #112.

A second audit on 14 November 2026 covered the parts the first one skipped: the `Failure` source generator, the MVC binders, the `If`/`MatchIf` families and the Testing packages. Its findings start at item 9 below. They were fixed on #121.

**What this repo does:** Wrap is a .NET library of monads: `Maybe` (a value that may be missing), `Result` (an operation that may have failed), `Either` (one of two values) and strongly typed `Id`s. Around them it adds a memory cache, JSON converters, MVC model binders, Dapper type handlers and a source generator that writes `Failure` methods.

**Load assumed:** web apps that use it under real traffic. The cache is registered as one shared instance (a singleton) for the whole app. The JSON converters and model binders read data sent by users.

**Verdict:** first audit all done, or decided (7). Second audit: 6 done, 1 withdrawn, 2 open (14 and 17).

**Lean:** about 30 lines left to remove (14), and no dependencies.

## Must fix

### 1. The cache's `LastFailure` is shared by every caller

- [x] Done

**Where:** `src/Caching/WrapCache.cs`, `src/Caching/IWrapCache.cs`

- **What this is:** When `GetOrCreate` fails, it returns `None` and stores the reason in the `LastFailure` property for you to read.
- **Problem:** `AddWrapCache` registers one cache for the whole app, so all requests share this one property. If request A's value factory throws and request B then reads any key, B overwrites `LastFailure` with `None` before A reads it. `Maybe<Failure>` is also a multi-field struct, so two threads writing it at once can leave half of one value mixed with half of the other.
- **Fix:** Return the failure with the call: make `GetOrCreate`/`GetOrCreateAsync` return `Result<TValue>`, and remove `LastFailure`. An `AsyncLocal` won't work, because changes made inside an `async` method don't flow back to the caller.
- **Done:** `GetValue`, `GetOrCreate` and `GetOrCreateAsync` all return `Result<TValue>`, and `LastFailure` is removed. A missing key is now a `Failure` with the message "Cache entry '{Key}' not found." The locking was later removed too (see 5).
- **If we skip it:** Under load, error details go missing or belong to another request.

### 2. JSON turns bad numbers from users into `0`

- [x] Done

**Where:** `src/All/Json/MaybeJsonConverter.cs` and `src/All/Json/MonadJsonConverter.cs`, the `catch` blocks

- **What this is:** These converters read `Maybe<T>` and the strong ID types from JSON.
- **Problem:** For value types, any read error returns the default value. Verified: `"abc"` and `1e99` both read as `Maybe<int>` with the value `0`. An ID type would come back as 0 or `Guid.Empty` the same way. `Deserialize_Tests` asserts this on purpose (`AssertSome(default)`), so it's a choice, but it treats invalid input from a user as a real value.
- **Fix:** Remove the value-type branch in both `catch` blocks, so the `JsonException` reaches the caller. If you'd rather not throw, return `None` instead of `Some(default)` in `MaybeJsonConverter`. Update `With_Invalid_Json` to match.
- **Done:** both value-type branches are removed, so invalid JSON throws `JsonException` for every type. JSON `null` is still `None`. This also restores the 1.x behaviour for nested unions such as `Maybe<Maybe<X>>`, which had started returning `Some(None)` once the unions became structs.
- **If we skip it:** A client sending `"id": "oops"` can read or update record 0 or `Guid.Empty` instead of getting a 400 error.

### 3. `F.Format` crashes on literal braces, which also breaks `Failure.ToString()`

- [x] Done

**Where:** `src/Common/Functions/F.Format.cs`

- **What this is:** `F.Format` fills `{Name}` placeholders. `Failure.ToString()`, `Result<T>.ToString()` and the `UnsafeUnwrapException` message all go through it.
- **Problem:** Verified: `R.Fail("Value {A} rejected by rule {\"max\":5}", 42).ToString()` throws `FormatException`. The JSON-like `{"max":5}` stays in the string passed to `string.Format`, which rejects it. The same thing happens if `Unsafe().Unwrap()` hits that failure: you get a `FormatException` instead of the `UnsafeUnwrapException` that tells you what failed.
- **Fix:** Put a `try`/`catch (FormatException)` around the final `string.Format` and return `formatString` unchanged.
- **Done:** instead of the `try`/`catch` alone, the replace step was rewritten to write the final text itself rather than passing the rewritten template to `string.Format`:
  - Braces outside placeholders are kept as written.
  - `{{name}}` is literal text and takes no argument.
  - Unsupported or invalid input (alignment such as `{A,3}`, unknown format specifiers, unbalanced braces) returns the format string unchanged, so formatting a message never throws.
  - Mixed numbered and named placeholders, and alignment, are not supported.
  - Covered by `tests/Tests.Common/Functions/F.Format_Forms_Tests.cs`, which has one row per input form, so a change to one form shows its effect on all the others.
- **If we skip it:** Building an error message, or logging it, can throw and hide the real failure.

## Should fix

### 4. `M.ParseDateTime` turns UTC times into server local time

- [x] Done

**Where:** `src/Maybe/Functions/M.ParseDateTime.cs:21`, `:25`

- **What this is:** It parses dates, and the MVC binder uses it for `DateTime` query and form values.
- **Problem:** `DateTimeStyles.None` converts a `Z` time to local time. Verified on a UK machine in summer time: `2026-07-01T10:00:00Z` came back as `11:00` with `Kind=Local`.
- **Fix:** Use `DateTimeStyles.RoundtripKind` in both `DateTime` overloads. That's one word each.
- **Done:** as above. `ParseDateTime_Tests` now builds its expected value with `RoundtripKind` too. Before that, the GMT row failed on machines that aren't on UTC, but passed on a UTC server.
- **If we skip it:** Timestamps shift by the server's time zone, and the shift differs between development machines and production.

### 5. The cache lock is skipped for parallel work inside a value factory

- [x] Done (locking removed)

**Where:** `src/Caching/WrapCache.cs`

- **What this is:** An `AsyncLocal` bit records which of the 64 locks this call chain already holds, so a factory can call the cache again without blocking on its own lock.
- **Problem:** Child tasks inherit that bit. If a factory runs `Task.WhenAll(GetOrCreateAsync(B), GetOrCreateAsync(B))`, and B shares a lock with the outer key, both children skip the lock and B's factory runs twice at once. Preventing exactly that is the lock's job. Verified: with keys 1 and 65, which share a lock, B's factory ran twice on `main`, at `086993a` and at HEAD. Two parallel calls made outside a factory ran it once.
- **Fix:** Add a `shortcut:` comment that names this limit. Or, for real "one creator per key", hold one `Lazy<Task<…>>` per key in a `ConcurrentDictionary`, which also removes the bit tracking.
- **Decision:** remove locking altogether. The same five scenarios were run against v1.2.10, the striped locks and a `ConcurrentDictionary` prototype. v1.2.10's single lock made callers take turns but never stopped duplicate work: 20 callers meant 20 factory runs. It also made every key wait for a slow factory, and it hung on nested calls. Mileage, the main user, only caches reads (database queries filtered by user), so a factory running twice under contention costs one extra query.
- **Done:** `WrapCache` was rebuilt from v1.2.10's layout with no lock, dictionary or `AsyncLocal`. A `shortcut:` comment says a factory can run more than once when calls arrive together. `GetOrCreate`/`GetOrCreateAsync` now take factories returning `T` or `Result<T>`, replacing `Maybe<T>`: a `Failure` from the factory reaches the caller unchanged and is not cached. Tests cover that a slow factory doesn't block other keys, and that nested calls for another key complete.
- **If we skip it:** It's rare, but an expensive factory runs more than once and the last write wins.

### 6. The Guid ID handler silently reads unreadable values as `Guid.Empty`

- [x] Done

**Where:** `src/Id.Dapper/GuidIdTypeHandler.cs`, `Parse`

- **What this is:** It reads Guid IDs from database rows.
- **Problem:** It calls `ToString()` and falls back to `Guid.Empty` on any parse failure. A `byte[]` column (common in SQLite or MySQL) gives the text `"System.Byte[]"`, so it reads as `Guid.Empty`. The int and long handlers throw in the same situation.
- **Fix:** Handle `byte[] { Length: 16 }` with `new Guid(bytes)`, and throw for anything else that won't parse, as `DefaultOrThrow` does.
- **Done:** 16-byte arrays are read with `new Guid(bytes)`. A `shortcut:` comment notes that this assumes `Guid.ToByteArray()` byte order, so bytes in RFC 4122 order (MySQL `UUID_TO_BIN`, Oracle `SYS_GUID`) read back as a different Guid. "Throw for anything else" was not done: `Parse_Tests` deliberately expects text that can't be parsed to become the default.
- **If we skip it:** Every row reads with the same empty ID, and a later save can write to the wrong record.

### 7. `Audit` hides failures in audit callbacks

- [x] Done (by design)

**Where:** `src/All/Extensions/Maybe/MaybeExtensions.Audit.cs:40,61,91,130` and `src/All/Extensions/Result/ResultExtensions.Audit.cs`, same lines

- **What this is:** `Audit` runs a callback, for example writing to an audit log, without changing the value.
- **Problem:** All 8 `catch { }` blocks throw the error away. If the audit log's database is down, nothing is written and nothing says so. This replaced the deleted logging calls and hasn't been confirmed as the intended behaviour. Verified: a callback that throws "db down" runs, and the caller never sees the error.
- **Fix:** Delete the `try`/`catch`, so the callback's exception reaches the caller. That deletes about 24 lines.
- **Decision:** by design. Audit is a side job and must never change the result. On `Maybe` there is nowhere to put an error, and letting it through would make an audit change the outcome of the action. Bringing back a static `F.LogFailure` hook was considered and rejected: it is global shared state, like the old `LastFailure`, and an audit function that needs to know about its own errors can catch them itself.
- **Done:** the docs now say so. The two main overloads in each of `MaybeExtensions.Audit.cs` and `ResultExtensions.Audit.cs` have a `<remarks>` line, and every other overload inherits it.
- **If we skip it:** Missing audit entries with no trace of why.

### 8. `UnwrapSingle` throws without going through `Unsafe()`

- [x] Done

**Where:** `src/All/Extensions/Unsafe/UnsafeExtensions.UnwrapSingle.cs` (was `src/All/Extensions/Maybe/MaybeExtensions.UnwrapSingle.cs`)

- **What this is:** It returns the single item of a `Maybe<IEnumerable<T>>`.
- **Problem:** `.Single()` throws `InvalidOperationException` for 0 or 2+ items. That breaks the rule that anything that can throw lives under `Unsafe()`. `Result` already has a safe `GetSingle` that returns a `Result`.
- **Fix:** Move it into `UnsafeExtensions`, or delete it and point users to `ToResult().GetSingle()`. Deleting removes 58 lines.
- **Done:** moved under `Unsafe()` and made generic over `IEither`, so it works for `Maybe`, `Result` and `Either`. It takes an `fLeft` fallback for a Left value. The fallback also lets C# work out the item type, which it can't do from the constraint alone. It reads only the first two items, and throws `UnsafeUnwrapSingleException` when the list doesn't have exactly one. There is one async overload, on `Task<Unsafe<…>>`, the same way as `UnwrapAsync`. Tests are in `tests/Tests.All/Extensions/UnsafeExtensions/`.
- **If we skip it:** A "safe" call can still crash the caller.

## Also fixed

Found and fixed after the audit, without their own entry above:

- `F.Format` used the first argument twice in mixed strings: `"Item {0} for {Key}"` gave `Item a for a`. It now decides once, with `HasOnlyNumberedTemplates`, whether the string is numbered or positional. Tests cover both orders.
- `F.Format` escaped braces (`{{x}}`) took an argument and shifted every later value. This is covered by the item 3 rewrite.
- `GetSingle` copied the whole list just to count it. It now reads at most two items with `Take(2)`.
- `Failure.Arg` had a dead `?? []`.

## Left out

Three smaller findings were not listed above:

- `M.ParseChar` converts its span to a string, which allocates. The one-line fix is `input is [var c] ? c : None`.
- The sync and async `GetOrCreate` bodies in `WrapCache` are near-copies, now about 12 lines each. Merging them would mean blocking on async code.
- The JSON converters detect empty input by matching English exception text. No clean fix was found: checking `reader.TokenType == None` doesn't work, because a reader passed in directly starts at `None` even when it holds valid JSON.

## Not checked

- In `src/All/Extensions`, the LINQ `SelectMany`, `Bind`, `Map`, `Filter`, `FilterBind`, `Iterate` and `Unwrap` families have now been read, and nothing was found. The bodies of the `If`/`MatchIf` overload families were not read past their first overloads.
- `apps/AppMvc`, the Testing packages and the shared CI workflows in `bfren/dotnet` were not read.
- The cache was not load-tested.
- NuGet packages were not built.

# Second audit

Repo audit of branch `116-run-maybetoresultextensionsselect-through-rtry`, 14 November 2026. Fixes are on `121-audit-fixes` (#121). The load assumed is the same as above.

## Must fix

### 9. Two `[Failure]` methods with the same name break every generated failure

- [x] Done

**Where:** `src/Generators/Failure/FailureGenerator.cs`, `AddSource`

- **What this is:** The generator writes one file per `[Failure]` method, named after the namespace, class and method.
- **Problem:** Overloads get the same file name, so Roslyn throws and the generator stops. Verified in a scratch project: `NotFound(int id)` plus `NotFound(string name)` gave warning CS8785, then CS8795 "must have an implementation" for every `[Failure]` method in the project.
- **Fix:** Name the file from the method's documentation ID, which includes the parameter types.
- **Done:** as above, with characters that aren't allowed in file names replaced by `_`. `Failures.cs` in `Tests.Generators` now has an overload pair, so the test project doesn't compile if this breaks.
- **If we skip it:** One innocent overload stops the whole project from compiling, and the error doesn't point at the cause.

### 10. `[Failure]` inside a generic class does not compile

- [x] Done

**Where:** `src/Generators/Failure/FailureGenerator.cs`, the containing-type loop

- **What this is:** The generator re-opens each containing class as `partial class <Name>`.
- **Problem:** It used `current.Name`, which drops the type parameters. For `Box<T>` it wrote a new class `Box`, and `Box<T>.Bad(T)` was left with no body (CS8795, verified).
- **Fix:** Write the name with its type parameters.
- **Done:** as above. `Failures.InGeneric<T>` and a test in `Generate_Tests` cover it.
- **If we skip it:** Users can't put failures in generic classes, and the error they get is confusing.

## Should fix

### 11. The generator redoes all its work on every keystroke

- [x] Done

**Where:** `src/Generators/Failure/MethodModel.cs`

- **What this is:** Incremental generators skip work when the model they produce equals the previous one.
- **Problem:** `MethodModel` held arrays (`Parameters`, `ContainingTypes`, `Diagnostics`), which records compare by reference. So the model never matched the previous run, and the IDE regenerated every `[Failure]` file after any edit.
- **Fix:** Hold values that compare by content.
- **Done:** a new internal `EquatableArray<T>` compares its items. `Incremental_Tests` checks that an unrelated edit reuses the cached output; it fails with "Output was Modified" on reference comparison.
- **If we skip it:** Typing gets slower in the IDE as the number of failures grows.

### 12. `If` and `MatchIf` took their true and false branches in opposite order

- [x] Done (breaking)

**Where:** `src/All/Extensions/Maybe/MaybeExtensions.MatchIf.cs`, `src/All/Extensions/Result/ResultExtensions.MatchIf.cs`

- **What this is:** `If(fTest, fTrue, fFalse)` compared with `MatchIf(fNone, fTest, fFalse, fTrue)`. 1.x had the same order.
- **Problem:** Both branches have the same type, so mixing up the order compiles. `m.MatchIf(none, test, a, b)` ran `a` when the test was false, while `m.If(test, a, b)` runs `a` when it is true.
- **Fix:** Use one order everywhere while 2.0 is still in beta.
- **Done:** every `MatchIf`/`MatchIfAsync` overload now takes `fTrue` before `fFalse`. Callers passing arguments by position must swap them; callers using named arguments are not affected.
- **If we skip it:** Inverted logic that neither the compiler nor a reviewer catches easily.

### 13. A bad MVC value silently becomes `None`

- [x] Done

**Where:** `src/All.Mvc/WrapModelBinder.cs:100-110`, `src/All.Mvc/MaybeModelBinder.cs`

- **What this is:** The MVC binder reads request values like `?userId=…` into `Maybe<T>` parameters.
- **Problem:** A value that fails to parse goes through the same `Nothing()` path as a missing one, and `MaybeModelBinder` reports a successful `None`. `GET /orders?userId=abc` behaves like "no filter", so an action that filters only when the value is present can return every order. The JSON converters were changed to reject bad input (item 2), so the two disagree.
- **Fix:** Keep `Nothing()` for missing or empty values. When parsing fails, add a ModelState error and return `ModelBindingResult.Failed()`, so `[ApiController]` returns a 400. Update the `Incorrect_Type` test, which asserts the current behaviour.
- **Done:** parse failures now go through a new `Failed(result)`, which keeps the raw value and fails binding, and `BindModelAsync` adds a ModelState error when binding failed for a value that is present. `Nothing()` is still used for missing or empty values, so `Maybe` still binds those as `None`. Tests cover both binders in `GetValue_Tests` and `BindModelAsync_Tests`.
- **If we skip it:** Typos and bad clients get wrong, wider results instead of an error.

### 14. `ToResult` catches exceptions that monad-returning lambdas should handle

- [ ] Open

**Where:** `src/All/Extensions/Maybe/MaybeExtensions.ToResult.cs`

- **What this is:** It turns a `Maybe` into a `Result`, with a `noneHandler` that returns a `Result`.
- **Problem:** The rule is that a lambda returning a monad handles its own exceptions, so `Bind` doesn't catch them. `ToResult` wraps all four overloads in `try`/`catch`, faulted tasks included. So `m.ToResult(() => throw x)` returns a `Failure`, but `m.Bind(_ => throw x)` throws.
- **Fix:** Delete the four `try`/`catch` blocks (about 30 lines), or document why `ToResult` is the exception.
- **If we skip it:** The same mistake behaves differently depending on which method it happens in.

### 17. The generator can't implement `params`, `in`/`ref`, generic or global-namespace methods

- [ ] Open

**Where:** `src/Generators/Failure/FailureGenerator.cs`, `src/Generators/Failure/ParameterModel.cs`

- **What this is:** The generator copies each parameter as `{type} {name}`, the method as `{name}(…)`, and the namespace from `ContainingNamespace.ToDisplayString()`.
- **Problem:** Verified in a scratch project on the #121 code. Each case fails to compile:
  - `params object[] items` loses `params`: CS0758.
  - `in int value` (likewise `ref`/`out`) loses its modifier: CS8795 and CS0759.
  - `Generic<T>(T value)` loses its type parameter: CS0246 and CS0759.
  - A class in the global namespace is generated into a different namespace: CS8795 and CS0759.
- **Fix:** Copy the parameter modifiers (`params`, `in`, `ref`, `out`, `scoped`) and the method's type parameters with their constraints, and leave out the `namespace` line when `ContainingNamespace.IsGlobalNamespace`. Or report WRAP001 for the cases that aren't supported, which is less code.
- **If we skip it:** Users get compiler errors pointing at generated code, with no diagnostic saying why.

## Nice to have

### 15. Two identical failures are not equal

- [x] Withdrawn

- **Claim:** `Failure` compared `Args` by reference.
- **Wrong:** `src/Result/Failure.Equatable.cs` already compares `Args` item by item. The audit missed that file.

### 16. `WrapCache` had a base class only so `nameof` worked

- [x] Done

**Where:** `src/Caching/WrapCache.cs`

- **What this is:** An empty public abstract class `WrapCache`, there only so that `nameof(WrapCache)` would compile.
- **Fix:** Use `nameof(WrapCache<>)` (C# 14) and delete the class.
- **Done:** as above. The failure context still reads `WrapCache`.

## Left out

- `F.DefaultCulture` is a global setting anyone can change, and it defaults to en-GB. The binder parses `03/04/2026` as 3 April, even for a US client.
- `M.ParseDateTimeOffset` assumes server local time when the input has no offset, so the result depends on the server's time zone.
- `Rnd.Int`/`Lng`/`UInt32`/`UInt64` are documented as covering the full range, but they return values of about 10,000–19,999 (the `limit: true` path in `IdGenerator` relies on them).
- From the first audit's "Left out", two items still apply: the `GetOrCreate` bodies in `WrapCache` are near-copies, and the JSON converters match English exception text. The third, `M.ParseChar` allocating, is already fixed: it now uses `input is [var c] ? c : None`.

All items above were rechecked against `121-audit-fixes` on 14 November 2026, and `main` had nothing newer.

### For later

Smaller things noticed during the second audit and not verified by running them. Look at these before deciding whether they need an item:

- **Int, long and uint ID handlers reject whole decimals.** `IntIdTypeHandler.Parse` and the others parse `value.ToString()`. A provider that returns `NUMBER` columns as `decimal` (for example Oracle) can give `5.0m`, which prints as `"5.0"`. That doesn't parse as an int, so `DefaultOrThrow` throws `OverflowException` even though the value fits. `ToString()` also uses the current culture.
- **The cache breaks when `IMemoryCache` has a `SizeLimit`.** `WrapCache` always passes `new MemoryCacheEntryOptions()` with no `Size`. With a size limit set, `MemoryCache` throws on every `Set`. So `SetValue` throws, and `GetOrCreate` returns a `Failure` on every call and never caches anything.
- **`F.Format` named placeholders use the static type.** `PropertyCache<T>` looks up properties on `typeof(T)`. `F.Format("{Name}", (object)user)` finds nothing and leaves `{Name}` in the text. `Failure.ToString()` passes an array, so it isn't affected; only direct callers are.
- **`default(Either<L, R>)` is `Left(default)`.** That matches `default(Maybe<T>)` being `None`, but it isn't documented on `Either` the way it is on `Maybe`.

## Not checked

- The `Enumerable`/LINQ families weren't re-read, since the first audit covered them.
- `apps/` and the shared CI workflows were not read.
