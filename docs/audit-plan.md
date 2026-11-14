# Audit plan

Repo audit of branch `108-switch-to-use-union-types`, 12 November 2026.

**What this repo does:** Wrap is a .NET library of monads: `Maybe` (a value that may be missing), `Result` (an operation that may have failed), `Either` (one of two values) and strongly typed `Id`s. Around them it adds a memory cache, JSON converters, MVC model binders and Dapper type handlers.

**Load assumed:** web apps that use it under real traffic. The cache is registered as one shared instance (a singleton) for the whole app. The JSON converters and model binders read data sent by users.

**Verdict:** fix 1, 2 and 3 first. Number 2 is a check on user input, and the other two hide real errors.

**Lean:** about 80 lines could go (numbers 7 and 8), and no dependencies.

## Must fix

### 1. The cache's `LastFailure` is shared by every caller

- [x] Done

**Where:** `src/Caching/WrapCache.cs:33`, `:158`, `:168`, `:265`

- **What this is:** When `GetOrCreate` fails, it returns `None` and stores the reason in the `LastFailure` property for you to read.
- **Problem:** `AddWrapCache` registers one cache for the whole app, so all requests share this one property. If request A's value factory throws and request B then reads any key, B overwrites `LastFailure` with `None` before A reads it. `Maybe<Failure>` is also a multi-field struct, so two threads writing it at once can leave half of one value mixed with half of the other.
- **Fix:** Return the failure with the call: make `GetOrCreate`/`GetOrCreateAsync` return `Result<TValue>`, and remove `LastFailure`. An `AsyncLocal` won't work, because changes made inside an `async` method don't flow back to the caller.
- **If we skip it:** Under load, error details go missing or belong to another request.

### 2. JSON turns bad numbers from users into `0`

- [x] Done

**Where:** `src/All/Json/MaybeJsonConverter.cs` and `src/All/Json/MonadJsonConverter.cs`, the `catch` blocks

- **What this is:** These converters read `Maybe<T>` and the strong ID types from JSON.
- **Problem:** For value types, any read error returns the default value. Verified: `"abc"` and `1e99` both read as `Maybe<int>` with the value `0`. An ID type would come back as 0 or `Guid.Empty` the same way. `Deserialize_Tests` asserts this on purpose (`AssertSome(default)`), so it's a choice, but it treats invalid input from a user as a real value.
- **Fix:** Remove the value-type branch in both `catch` blocks, so the `JsonException` reaches the caller. If you'd rather not throw, return `None` instead of `Some(default)` in `MaybeJsonConverter`. Update `With_Invalid_Json` to match.
- **If we skip it:** A client sending `"id": "oops"` can read or update record 0 or `Guid.Empty` instead of getting a 400 error.

### 3. `F.Format` crashes on literal braces, which also breaks `Failure.ToString()`

- [x] Done

**Where:** `src/Common/Functions/F.Format.cs:85`

- **What this is:** `F.Format` fills `{Name}` placeholders. `Failure.ToString()`, `Result<T>.ToString()` and the `UnsafeUnwrapException` message all go through it.
- **Problem:** Verified: `R.Fail("Value {A} rejected by rule {\"max\":5}", 42).ToString()` throws `FormatException`. The JSON-like `{"max":5}` stays in the string passed to `string.Format`, which rejects it. The same thing happens if `Unsafe().Unwrap()` hits that failure: you get a `FormatException` instead of the `UnsafeUnwrapException` that tells you what failed.
- **Fix:** Put a `try`/`catch (FormatException)` around the final `string.Format` and return `formatString` unchanged. `Check` already does this on line 119.
- **If we skip it:** Building an error message, or logging it, can throw and hide the real failure.

## Should fix

### 4. `M.ParseDateTime` turns UTC times into server local time

- [x] Done

**Where:** `src/Maybe/Functions/M.ParseDateTime.cs:21`, `:25`

- **What this is:** It parses dates, and the MVC binder uses it for `DateTime` query and form values.
- **Problem:** `DateTimeStyles.None` converts a `Z` time to local time. Verified on a UK machine in summer time: `2026-07-01T10:00:00Z` came back as `11:00` with `Kind=Local`.
- **Fix:** Use `DateTimeStyles.RoundtripKind` in both `DateTime` overloads. That's one word each.
- **If we skip it:** Timestamps shift by the server's time zone, and the shift differs between development machines and production.

### 5. The cache lock is skipped for parallel work inside a value factory

- [ ] Done

**Where:** `src/Caching/WrapCache.cs:37`, `:140`, `:207`

- **What this is:** An `AsyncLocal` bit records which of the 64 locks this call chain already holds, so a factory can call the cache again without blocking on its own lock.
- **Problem:** Child tasks inherit that bit. If a factory runs `Task.WhenAll(GetOrCreateAsync(B), GetOrCreateAsync(B))`, and B shares a lock with the outer key, both children skip the lock and B's factory runs twice at once. Preventing exactly that is the lock's job.
- **Fix:** Add a `shortcut:` comment that names this limit. Or, for real "one creator per key", hold one `Lazy<Task<…>>` per key in a `ConcurrentDictionary`, which also removes the bit tracking.
- **If we skip it:** It's rare, but an expensive factory runs more than once and the last write wins.

### 6. The Guid ID handler silently reads unreadable values as `Guid.Empty`

- [x] Done

**Where:** `src/Id.Dapper/GuidIdTypeHandler.cs`, `Parse`

- **What this is:** It reads Guid IDs from database rows.
- **Problem:** It calls `ToString()` and falls back to `Guid.Empty` on any parse failure. A `byte[]` column (common in SQLite or MySQL) gives the text `"System.Byte[]"`, so it reads as `Guid.Empty`. The int and long handlers throw in the same situation.
- **Fix:** Handle `byte[] { Length: 16 }` with `new Guid(bytes)`, and throw for anything else that won't parse, as `DefaultOrThrow` does.
- **If we skip it:** Every row reads with the same empty ID, and a later save can write to the wrong record.

### 7. `Audit` hides failures in audit callbacks

- [ ] Done

**Where:** `src/All/Extensions/Maybe/MaybeExtensions.Audit.cs:40,61,91,130` and `src/All/Extensions/Result/ResultExtensions.Audit.cs`, same lines

- **What this is:** `Audit` runs a callback, for example writing to an audit log, without changing the value.
- **Problem:** All 8 `catch { }` blocks throw the error away. If the audit log's database is down, nothing is written and nothing says so. This replaced the deleted logging calls and hasn't been confirmed as the intended behaviour.
- **Fix:** Delete the `try`/`catch`, so the callback's exception reaches the caller. That deletes about 24 lines.
- **If we skip it:** Missing audit entries with no trace of why.

### 8. `UnwrapSingle` throws without going through `Unsafe()`

- [x] Done

**Where:** `src/All/Extensions/Maybe/MaybeExtensions.UnwrapSingle.cs`

- **What this is:** It returns the single item of a `Maybe<IEnumerable<T>>`.
- **Problem:** `.Single()` throws `InvalidOperationException` for 0 or 2+ items. That breaks the rule that anything that can throw lives under `Unsafe()`. `Result` already has a safe `GetSingle` that returns a `Result`.
- **Fix:** Move it into `UnsafeExtensions`, or delete it and point users to `ToResult().GetSingle()`. Deleting removes 58 lines.
- **If we skip it:** A "safe" call can still crash the caller.

## Left out

Three smaller findings were not listed above:

- `M.ParseChar` converts its span to a string, which allocates.
- The sync and async `GetOrCreate` bodies in `WrapCache` are near-copies.
- The JSON converters detect empty input by matching English exception text.

## Not checked

- The bulk of `src/All/Extensions` (about 4,500 lines: Bind, Map, Filter, LINQ and the async variants) was only skimmed for risky patterns, not read method by method.
- `apps/AppMvc` was not read.
- The cache was not load-tested.
- NuGet packages were not built.
