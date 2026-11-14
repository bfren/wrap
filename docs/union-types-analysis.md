# Wrap and C# 15 union types: analysis, recommendations, and plan

*Written 11 November 2026, the day after .NET 11 / C# 15 went GA. I checked every claim marked ✅ against SDK 11.0.100 on this machine. The prototypes are listed in the appendix.*

---

## TL;DR

- **Unions can replace how Wrap *stores* its types, not what Wrap *does* with them.** A union is a sum type: it holds one of a fixed set of cases and gives you exhaustive `switch`. It has no `Bind`, `Map`, LINQ, async chaining, `Audit`, `Filter`, logging or parsing. In this codebase that "monad" layer is the ~5,400 lines in `src/All`, plus the integrations, and all of it stays.
- **What unions *can* replace** is the representation layer: `Maybe`/`Some`/`NoneImpl`, `Result`/`Ok`/`FailureImpl`, `Either`/`Left`/`Right`, the `IEither`/`ILeft`/`IRight` plumbing, the runtime `Invalid…TypeException`/`Null…Exception` fallbacks, and most of `Unsafe`. That is roughly 1,000–1,500 lines and about 51 internal pattern-match sites.
- **It is still worth doing, as a v2.0.** You gain compile-time exhaustiveness for your consumers, public pattern-matchable cases (today `NoneImpl`/`FailureImpl` are internal), **zero allocations** for `Some`/`Ok` (today every one is a heap object), a `default` that safely means `None`, and a smaller core. The cost is a hard breaking change: `Maybe<T>` and `Result<T>` change from reference types to structs.
- **Recommended shape:** hand-written, non-boxing `[Union] readonly struct`s whose cases are small record structs: `Some<T>`/`None`, `Ok<T>`/`Failure`, `Left<T>`/`Right<T>`. **Don't** use the one-line `union X(A, B)` shorthand for the core types: it boxes value-type cases, makes `default` a runtime crash, and `Either<int,int>` won't compile.
- **You don't have to drop .NET 8–10 at runtime.** ✅ A union library compiles and runs on `net8.0` with a two-type internal polyfill (`UnionAttribute`, `IUnion`), including across assembly boundaries. The real requirement is the **compiler**: consumers need the .NET 11 SDK and `LangVersion` 15 (the C# version that ships with .NET 11). For net8–10 projects that is a manual, officially unsupported setting. Microsoft only supports net11+, so net11-only is the honest, supported choice. The polyfill exists if you want it.
- **Decided (11 November 2026): Wrap 2.0.0 targets .NET 11+ only and goes all-in on union types.** The rest of this document assumes that decision. The polyfill route is recorded in section 6 as considered and rejected.
- **Implemented (12 November 2026):** the core types are unions on branch `108-switch-to-use-union-types` (`2.0.0-beta.1`). See section 0 for where things stand and section 10 for decisions made during the work.

---

## 0. Status (13 November 2026)

Branch `108-switch-to-use-union-types`, version `2.0.0-beta.1`. The solution builds with no warnings and all 1,508 tests pass on `net11.0`.

| Phase | Status |
|---|---|
| 0. Spike and decisions | **Done.** Section 9 decisions made. BenchmarkDotNet compares v1 and v2 in `apps/AppBenchmarkv1` and `apps/AppBenchmarkv2`, and the results are in `apps/Benchmarks/README.md`. |
| 1. Groundwork | Not done as a separate step. The representation was funnelled during the rewrite. No `[Obsolete]` markings were added and there is no final 1.x release (the latest tag is `v1.4.0`). |
| 2. Core types | **Done.** `Maybe<T>`, `Result<T>` and `Either<TLeft,TRight>` are `[Union] readonly partial struct`s. `Some<T>`, `Ok<T>`, `Left<T>`, `Right<T>`, `None` and `Failure` are `readonly record struct`s. `NoneImpl`, `FailureImpl` and the `Invalid…`/`Null…` exceptions are gone. `src` and `tests` target `net11.0` only. |
| 3. Combinators | **Done.** The bodies in `src/All` use the union cases. `IEither` and `Unsafe` were kept (section 10). |
| 4. Integrations | **Done** except the app. The JSON converters, MVC binders, Caching and the Testing packages are adapted, and their tests pass. `apps/AppMvc` has not been smoke-tested. |
| 5. Tests, docs, release | **In progress.** Tests are updated and XML docs rewritten. The rest is listed below. |

**Decided (12 November 2026)**
- **Keep a 1.x maintenance branch** (targets .NET 8, 9, 10 and 11) for consumers not ready for 2.x. It gets fixes only. This replaces the optional final 1.x release with `[Obsolete]` markings from Phase 1, which won't happen.
- **Downstream use of `IEither` and `Unsafe` is minimal**, so the migration guide only needs to mention them briefly.
- **CI keeps installing SDKs 8, 9, 10 and 11.** The shared workflow (`bfren/dotnet`) installs all four by default, but it builds and tests `Wrap.slnx`, which targets `net11.0` only, and `global.json` pins SDK 11. The extra SDKs only add setup time. To skip them, pass `dotnet-versions: 11.0.x` from `.github/workflows/test.yml`.
- **Hold the docs and migration guide until release.**

**Done since the first status update**
- `Maybe<T>.GetEnumerator` and `Unsafe.GetEnumerator` use `yield` (see section 10 for why `[]`/`[x]` was reverted).
- `Either` has an `.Unsafe()` extension, with tests.
- `global.json` pins SDK `11.0.100` (`rollForward: latestFeature`).
- `CLAUDE.md` and the README describe .NET 11 only, and `CLAUDE.md` uses `dotnet test --solution Wrap.slnx`.
- BenchmarkDotNet baselines for v1 and v2 (`apps/Benchmarks/README.md`).
- Exhaustiveness is pinned: `src` and `tests` treat CS8509 and CS8655 as errors, so a union change that breaks a `switch` with no `_` arm fails the build. A separate consumer-style project wasn't needed.
- `IEither<TEither,TLeft,TRight>` and `IEither.Unwrap` are gone (section 10).

**Still to do**
- Allocation regression tests (Phase 5). A unit test using `GC.GetAllocatedBytesForCurrentThread` would be flaky, because unoptimised tier-0 code boxes (section 5), so this probably means checking the benchmark results.
- Replace the remaining capturing lambdas in `src/All` with direct `switch` bodies: `ToMaybe`, `Maybe.Unwrap(Func<None,T>)` and `MaybeJsonConverter.Write`.
- Smoke-test `apps/AppMvc`.
- At release: move `PackageValidationBaselineVersion` from `1.2.1` to `2.0.0` once 2.0.0 ships, create the 1.x branch, and write the wiki docs and migration guide (section 8, Phase 5).

---

## 1. Unions as shipped in C# 15 (verified)

| Aspect | Behaviour |
|---|---|
| Shorthand | `public union Pet(Cat, Dog, Bird);` lowers to a plain `struct` with `object? Value`, one public constructor per case, and `IUnion`. Generic unions, a body with members, `partial` and interfaces are all allowed. Instance fields and auto-properties are not allowed in a shorthand union. |
| Case types | Any type that converts to `object`, type parameters included. ✅ `union Either<L,R>(L, R)` compiles, but **using `Either<int,int>` fails with CS0457 (ambiguous conversion)**. |
| Storage | Shorthand unions keep the value in one `object?` field. ✅ Value-type cases **box**: 24 bytes for every `Opt<int>` assignment. |
| Custom ("non-boxing") unions | `[Union]` on your own struct, public single-parameter constructors (these define the cases), `object? Value`, plus optional `bool HasValue` and `bool TryGetValue(out Case)` per case. ✅ Type patterns call `TryGetValue` rather than `Value`, so you get **0 bytes per op**. |
| Conversions | There is an implicit conversion from every case type. ✅ Your own `implicit operator` in the body wins over it (an `implicit operator Maybe<T>(T)` works). |
| Exhaustiveness | Covering every case type is exhaustive with no `_` arm. ✅ This works in generic methods when the cases are non-null types (`Some<T>`, `None`). With a raw `T` case and unconstrained `T` you get **CS8655** (null not handled). |
| `default` | In a shorthand union, `default` has `Value == null`. ✅ A switch with no `null` arm compiles **with no warning** and then **throws `SwitchExpressionException` at runtime**. A custom union can make `default` mean `None` (✅ verified). |
| Equality / ToString | **Not generated.** ✅ `ToString()` returns `` Maybe`1[System.Int32] ``. Equality falls back to reflection-based `ValueType.Equals`. You write both yourself, as Wrap already does. |
| Closed hierarchies | `public closed record Shape;` makes the class implicitly abstract (✅ adding `abstract` is an error, CS9384), and derived types are limited to the same assembly. ✅ A switch over the derived types is exhaustive. |
| BCL types | **No `Option`, `Result` or `Either` in .NET 11.** The docs' examples hand-roll `None`/`Some<T>`. Wrap still has a reason to exist. |
| System.Text.Json | Supported natively. ✅ It writes the case value: a shorthand `Maybe` gives `{"Value":5}` for Some and **`{}` for None**, and a custom union gives `5` / `{}`. ✅ **Deserialising fails** when cases share a JSON shape unless you configure a classifier. |
| ASP.NET Core | Supports unions in JSON bodies (Minimal APIs, MVC) and emits OpenAPI `anyOf`. **Route, query, header and form binding are not covered**, so your MVC model binders stay. |
| LINQ / async | Nothing built in. ✅ Your own `Select`/`SelectMany` extensions over a union work with query syntax. C# 14 `extension` blocks also work on unions. |

---

## 2. What Wrap is made of today

| Layer | Projects | Lines | Do unions affect it? |
|---|---|---:|---|
| **Representation** | `Maybe` (950), `Result` (1,057), `Either` (390), `Common` (738) | ~3,100 | **Partly.** The case classes, `NoneImpl`/`FailureImpl`, `IEither`/`ILeft`/`IRight`, `Invalid…`/`Null…` exceptions and the switch-with-throw `Match` bodies go away. The parsers (`M.Parse*`), `F.Format`, logging hooks, `FailureValue` and the `Failure` builder (`Msg`/`Ctx`/`Lvl`/`Arg`) stay. |
| **Combinators ("the monad")** | `All` | 5,439 | **No.** About 250 public overloads (`Bind`, `Map`, `Match`, `MatchIf`, `Audit`, `If`, `Filter`, `Unwrap`, `ToMaybe`/`ToResult`, LINQ, enumerable helpers, `…Async` variants) stay. Their bodies get simpler (`switch` on cases), and `Unsafe` is mostly replaced by native patterns. |
| **Integrations** | `All.Mvc` (460), `Caching` (371), JSON in `All` | ~1,000 | **Light rework.** The converters and binders target the new struct types. The binders are still needed. The converters are still needed if you keep today's JSON format (None → `null`). |
| **Strong IDs** | `Id`, `Id.Dapper` | ~510 | **No.** `Id<TId,TValue>` is a single-value wrapper, not a sum type. Unions have nothing to offer it. |
| **Testing** | `*.Testing` | ~400 | Rewrite the internals and keep the API (`AssertSome`, `AssertOk`, `AssertFailure`…). |
| Tests | 262 files | ~22,000 | Mostly mechanical changes. Only 4 files touch internals (`NoneImpl`/`FailureImpl`, 14 references). |

---

## 3. What you gain

1. **Compile-time exhaustiveness for consumers.** `maybe switch { Some<int> s => …, None => … }` is checked by the compiler. Today the only safety net is the runtime `throw Invalid(...)` arm in every `Match`.
2. **Public, pattern-matchable cases.** `NoneImpl` and `FailureImpl` are `internal` today, so consumers *can't* write `result is Failure f`. With unions, `if (result is Ok<User>(var user))` and `result switch { Failure f => … }` are just C#.
3. **Zero-allocation `Some`/`Ok`.** ✅ Measured: a custom `[Union]` struct allocates 0 B/op. Today's `Some<T>`/`Ok<T>` are `sealed record class`es, so every wrap is a heap allocation, and so is every `Map`/`Bind` step in a chain. This is the biggest performance win available to the library.
4. **`default` stops being dangerous.** `default(Maybe<T>)` can be defined as `None`. Today a `null` `Maybe<T>` reference is possible and is caught only at runtime (`NullMaybeException`).
5. **A smaller, simpler core.** You can delete `NoneImpl`, `FailureImpl`, `IEither<,>`, `IEither<,,>`, `ILeft`, `IRight`, `InvalidTypeException` and its three subclasses, the three `Null…Exception`s, most of `Unsafe<,,>` with `TryOk`/`TrySome`/`TryFailure` (replaced by `if (r is Ok<T>(var v))`), and the reflection in `GetGenericTypeArguments` that exists only for the `IEither` family.
6. **Platform alignment.** System.Text.Json, ASP.NET Core and OpenAPI recognise `IUnion` types, and the IDE understands them. You're building on the language's own vocabulary rather than a parallel one.

## 4. What you lose or must actively preserve

1. **Reference-type semantics (the main breaking change).** `Maybe<T>`/`Result<T>` become structs. Consumer code that does `maybe is null`, uses `Maybe<T>?` to mean "missing Maybe", relies on inheritance, or uses `Some<T>`/`Ok<T>` as reference types breaks. `Some<T>`/`Ok<T>` become record structs too.
2. **Pre-.NET 11 consumers, unless you polyfill.** See section 6.
3. **The polymorphic `IEither` abstraction.** `E.Match<TEither,TLeft,TRight>`, `EitherExtensions` and `Unsafe<TEither,…>` work across Maybe, Result and Either through interfaces. Unions don't need that, and a struct used through an interface boxes. You lose the ability to write one generic function over "any Wrap either". It looks like an internal convenience rather than something consumers use, but check your downstream projects.
4. **Free record behaviour.** You get no generated equality, `ToString`, `with` or deconstruction on the union itself. Wrap already overrides `Equals`/`GetHashCode`/`ToString`/`==` by hand, so you mostly carry existing code across. Without it, `ToString()` returns the type name and equality goes through reflection.
5. **Logging on failure creation needs care.** `FailureImpl`'s `init` accessor calls `F.LogFailure`/`F.LogException`, and `R.Pass` builds a non-logging copy. Move that into the `Result<T>(Failure)` constructor, with an internal non-logging constructor for pass-through. It is easy to lose this in a rewrite, so add a test first.
6. **JSON wire format.** Native STJ writes None as `{}`. Today it is `null`. Keep `MaybeJsonConverter` if you want existing payloads and clients to keep working. Also confirm that a registered converter takes precedence over the built-in union handling (not yet verified; see the Phase 0 spike).
7. **Some shorthand pitfalls stay real for anyone who uses `union X(...)` directly:** silent runtime throw on `default`, boxing, and type-parameter cases that collide. This is why I recommend the custom pattern for the core types.
8. **Nullable analysis is not a guarantee.** The docs say plainly that it "doesn't guarantee that a generated union has non-null contents at runtime". Keep the null-guarding constructors (`M.Wrap`, `R.Wrap`).

---

## 5. Design options

| | A. Shorthand `union Maybe<T>(None, Some<T>)` | **B. Custom `[Union]` struct with record-struct cases (recommended)** | C. Custom struct with raw `T` as a case (`maybe is int x`) |
|---|---|---|---|
| Code size | Smallest | Small (~40 lines per type) | Small |
| Allocations | `Some<T>` record class: same as today. Struct cases box. | **0** ✅ | **0** ✅ |
| `default` | `null`: runtime throw, no warning ✅ | **`None`** ✅ | **`None`** ✅ |
| Generic exhaustiveness | ✅ | ✅ (no CS8655 for unconstrained `T`) | ⚠️ CS8655 unless `T : notnull`, which would ban `Maybe<string?>`-style use |
| `Either<int,int>` | ❌ CS0457 | ✅ (`Left<int>` ≠ `Right<int>`) | ❌ ambiguous |
| `Maybe<None>` / `Result<Failure>` | OK | OK | ❌ ambiguous |
| Familiar to current users | Medium | **High** (`Some<T>`, `Ok<T>`, `Failure`, `None` keep their names) | Low |

**Option B** keeps your vocabulary and is allocation-free. It gives consumers exhaustiveness in generic code and makes `default` safe. It costs about 40 lines of hand-written members per type, which replaces more than that. Sketch:

```csharp
public readonly record struct None;
public readonly record struct Some<T>(T Value);

[Union]
public readonly partial struct Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly T? value;
    private readonly bool isSome;

    public Maybe(Some<T> some) => (value, isSome) = (some.Value, some.Value is not null);
    public Maybe(None _) { }

    public object? Value => isSome ? new Some<T>(value!) : new None(); // only reflection/STJ boxes
    public bool HasValue => true;                                       // default == None, never null
    public bool TryGetValue(out Some<T> some) { some = new(value!); return isSome; }
    public bool TryGetValue(out None none)    { none = default;     return !isSome; }

    public static implicit operator Maybe<T>(T? v) => IsNull(v) ? new None() : new Some<T>(v!);
    // + Equals/GetHashCode/==/ToString/GetEnumerator carried over from today
}
```

> ✅ `some.Value is not null` inside the constructor is allocation-free once the JIT optimises the method (0 B/op for `int`, `int?` and `string`). Unoptimised tier-0 code boxes value-type `T` (24 B/op for `int`), and reading the value into a local doesn't change that. Earlier drafts of this document mistook the tier-0 result for a real pitfall. Benchmark every core type with warm-up (BenchmarkDotNet's default) so tier-0 code isn't measured.

`Result<T>` follows the same pattern with `Ok<T>`/`Failure`: the `Failure` constructor logs, and an internal two-parameter constructor skips logging for `R.Pass`. `Either<TLeft,TRight>` uses `Left<T>`/`Right<T>`.

---

## 6. The ".NET < 11" question

> **Decision: net11-only, released as v2.0.0.**

| Path | Runtime TFMs | Consumer requirement | Support status |
|---|---|---|---|
| **net11-only (chosen)** | `net11.0` (+ `net12.0` later) | .NET 11 SDK, default `LangVersion` | Supported |
| Polyfill multi-target (rejected) | `net8.0;net9.0;net10.0;net11.0` | .NET 11 SDK **and** `<LangVersion>15</LangVersion>` in the consumer's project. ✅ C# 12 consumers get CS9202 ("Feature 'unions' is not available") as soon as they touch a union conversion or pattern. | Works ✅, but a newer language version on an older TFM is officially unsupported |

The polyfill is two `internal` types behind `#if !NET11_0_OR_GREATER`, and it worked across assemblies in testing. It doesn't buy much, though. Consumers on net8–10 still have to change their SDK and `LangVersion`, and anyone who doesn't gets a library they can't use idiomatically. If you want to keep older runtimes, a cleaner split is **1.x maintenance branch for net8–10, 2.x for net11+**. One thing to know: .NET 10 is LTS, while .NET 11 is STS (STS releases now get 24 months, so both are currently supported until about late 2028). Some of your consumers may stay on 10 for a while.

---

## 7. Recommendation

**Do it, as Wrap 2.0.0, using Option B on net11+ only (decided).** Keep the public vocabulary (`M`, `R`, `E`, `Some`, `None`, `Ok`, `Failure`, `Match`, `Bind`, `Map`, …) and every combinator in `All`. Treat unions as a new engine under the same API, not a replacement for the API. Before committing, decide the five questions in section 9. Keep `Id` exactly as it is.

**Don't** try to make unions "replace the monad functionality". They don't provide it, and there is no BCL `Option`/`Result` to defer to.

---

## 8. Phased plan

Each phase leaves `main` green and releasable.

### Phase 0: Spike and decisions (small)
- Branch, pin SDK 11 in `global.json`, and add BenchmarkDotNet baselines for wrap/`Map`/`Bind`/`Match` on today's code (allocations and time).
- Port `Maybe<T>` alone to Option B in the branch. Check: the JSON converter takes precedence over STJ's built-in union handling, nullable-reference `T` (`Maybe<string?>`), the `Maybe<object>` edge case, `foreach` via `GetEnumerator`, equality and `==` with `T`, and async chains with `Task<Maybe<T>>`.
- Answer the five decisions in section 9.

### Phase 1: Groundwork, non-breaking (on the 2.0 branch; optionally also a final 1.x release)

To warn 1.x users before 2.0.0, ship the `[Obsolete]` markings below in one last 1.x release (1.5, since 1.4.0 has shipped). Otherwise this phase is just preparation inside the 2.0 work.
- **Funnel the representation.** Replace the ~51 direct `is Some<T>` / `is Ok<T>` / `FailureImpl` / `NoneImpl` / `ILeft`/`IRight` checks in `src/All` and the integrations with a few internal helpers (`M.Match`/`R.Match` already do most of this). After this, the type swap in Phase 2 touches only a handful of files.
- Add tests that pin behaviour you must preserve: failure logging fires once and `R.Pass` doesn't re-log, None → `null` JSON, `ToString` formats, equality including `==` with `T`, and `M.Wrap(null)` → None.
- Mark APIs that will go as `[Obsolete]`, with messages pointing to the replacement pattern: `IEither`/`ILeft`/`IRight`, `E.Match<TEither,…>`, `Unsafe()`/`TryOk`/`TrySome`/`TryFailure`, the `Invalid…`/`Null…` exceptions.
- Optional and independent of unions: move the extension classes to C# 14 `extension` blocks. This works today on net8+.

### Phase 2: Core types (2.0.0-preview.1, breaking)
- `TargetFrameworks` → `net11.0` only in `src/Directory.Build.props` and `tests/Directory.Build.props`. Set `Version` to `2.0.0-preview.1`. Pin SDK 11 in `global.json`. Update the shared CI workflow matrix (drop 8/9/10), the "Supported .NET Versions" section in `CLAUDE.md`, and the README. Review `PackageValidationBaselineVersion` (1.2.1): validating against a 1.x baseline will flag every intended break, so move it to 2.0.0 once that ships.
- Rewrite `Maybe<T>`, `Result<T>` and `Either<TLeft,TRight>` as `[Union] readonly partial struct`s. Rewrite `Some<T>`, `Ok<T>`, `Left<T>`, `Right<T>` as `readonly record struct`s. Keep `None` and `Failure` as they are (already structs).
- Carry over equality, `ToString`, operators and `GetEnumerator`. Move failure logging into the `Failure` constructor.
- Delete `NoneImpl`, `FailureImpl`, the `IEither` family, the `Invalid…`/`Null…` exceptions and the `_ => throw Invalid(...)` arms.
- Decide what `Monad<T>` does. The `implicit operator … (Monad<T>)` conversions can stay as user-defined operators in the union bodies.

### Phase 3: Combinators (`src/All`)
- Re-implement the bodies on `switch` over the cases. Signatures don't change, so LINQ, async and `Audit` keep working for consumers.
- Remove `Unsafe` (or keep a thin, obsolete shim for one release). Native `is Ok<T>(var v)` replaces it.
- Re-run the benchmarks. Expect allocations to fall to the delegate and `Task` costs only.

### Phase 4: Integrations
- JSON: adapt `MaybeJsonConverter` and keep `null` for None (recommended). If you'd rather adopt STJ's native union handling, schedule that as its own breaking change with a classifier.
- MVC: adapt `MaybeModelBinder`/`WrapModelBinder` (ASP.NET doesn't bind unions from route, query or form) and `HiddenForMonad`.
- Caching: `GetValue`, `GetOrCreate` and `GetOrCreateAsync` return `Result<TValue>`, so each call carries its own failure. The shared `LastFailure` property is removed (it was overwritten by concurrent callers).
- Testing packages: re-implement `AssertSome`/`AssertNone`/`AssertOk`/`AssertFailure`/`AssertTrue`/`AssertFalse` and the generators. Their APIs don't change.
- `apps/AppMvc`: update and smoke-test.

### Phase 5: Tests, docs, release
- Update the 262 test files. The changes are mostly mechanical, and only 4 files reference internals.
- Add tests for: compile-time exhaustiveness (a consumer-style test project with `<WarningsAsErrors>CS8509;CS8655</WarningsAsErrors>`), `default(Maybe<T>) == None`, `default(Result<T>)` (decide what it means: probably a failure with a clear message), and zero-allocation benchmarks as regression guards.
- Write a wiki migration guide (.NET 11 requirement, struct semantics, `is null` → `IsNone`, `Unsafe().TryOk(out x)` → `is Ok<T>(var x)`, removed interfaces), then release 2.0.0-preview → 2.0.0. Optionally keep a `1.x` branch for fixes while consumers on .NET 10 LTS migrate.

---

## 9. Decisions for you

1. ~~**Runtime targets:** net11-only or the polyfill multi-target?~~ **Decided: net11-only, released as v2.0.0.**
2. ~~**Case shape:** `Some<T>`/`Ok<T>` record-struct cases (recommended) or raw `T` cases (`maybe is int x`)?~~ **Decided: record-struct cases (Option B).** `Some<T>`, `Ok<T>`, `Left<T>` and `Right<T>` are `readonly record struct`s.
3. ~~**JSON:** keep `null` for None with the custom converters (recommended) or switch to STJ native (`{}` for None, which breaks the wire format)?~~ **Decided: keep the custom converters.** `MaybeJsonConverter` reads and writes None as JSON `null`.
4. ~~**Either:** keep it as a `Left<T>`/`Right<T>` union, or drop it?~~ **Decided: keep it** as a `Left<T>`/`Right<T>` union. `default(Either<TLeft,TRight>)` is a `Left<T>` holding `default(TLeft)`.
5. ~~**`default(Result<T>)`:** a failure ("uninitialised Result") or a thrown exception on access?~~ **Decided: a failure.** `default(Result<T>)` is a `Failure` with the message `Uninitialised Failure`.

---

## 10. Decisions made during implementation (12 November 2026)

These change or override parts of sections 4, 7 and 8.

### Keep `IEither` and `Unsafe`

Sections 3, 4 and 8 planned to delete or obsolete both. **They stay.**

- **`Unsafe` is a deliberate speed bump.** Every way of reading a value through it drops the Left (failure) path: `Unwrap` throws `UnsafeUnwrapException`, `Discard` returns None, and `foreach` doesn't run. Calling `.Unsafe()` first makes that choice visible at the call site. `.Unsafe()` now exists for `Maybe`, `Result` and `Either` (the Either extension is new).
- **`IEither` exists only to support `Unsafe`.** In `src`, the only users are `Unsafe<TEither,TLeft,TRight>` itself (`GetEnumerator`) and four extensions: `Unwrap`, `UnwrapAsync`, `Discard` and `DiscardAsync`. It lets one `Unsafe` type serve all three unions instead of three copies. Its `Match` member has since been replaced (see below). The `Unwrap` and `AsTask` members, and with them the three-parameter `IEither<TEither,TLeft,TRight>`, were removed because nothing called them through the interface. Each union keeps its own public `Unwrap` and `AsTask`.
- **It doesn't box.** Section 4.3 says a struct used through an interface boxes. That is true only when the struct is stored *as* the interface. `Unsafe` and its extensions use generic constraints (`where TEither : IEither<…>`), which call through without boxing.
- `TryOk`, `TrySome` and `TryFailure` are gone, as planned. `is Ok<T>(var x)` replaces them.

### `IEither.Match` replaced by `TryGetLeft` / `TryGetRight` (13 November 2026)

`IEither<TLeft,TRight>.Match` made every `Unsafe` operation call two delegates. Maybe's first adapter was worse still: it bridged `M.Match`'s `Func<TReturn>` with `() => fLeft(default)`, which allocated a closure and a delegate (80 B) on every call.

`Match` is now gone from the interface. It is replaced by `bool TryGetLeft(out TLeft)` and `bool TryGetRight(out TRight)`, which read the private fields directly. Maybe, Result and Either implement them explicitly, so their own public surface doesn't change. This breaks any downstream code that called `IEither.Match`, which is believed to be minimal. `Unsafe.GetEnumerator`, `Unwrap` and `Discard` use them, and `Unwrap` keeps its throw in a separate helper so the success path can be inlined. BenchmarkDotNet, each benchmark run on its own:

| | Through `Match` | **`TryGetRight`** | v1 |
|---|---:|---:|---:|
| `maybe.Unsafe().Unwrap()` | 1.3 ns | **~0 ns** | 0.49 ns |
| `foreach` over `maybe.Unsafe()` | 2.6 ns | **~0 ns** | 0.48 ns |
| `result.Unsafe().Unwrap()` | 2.2 ns | **0.18 ns** | 0.49 ns |

None of them allocate.

### `GetEnumerator` uses `yield` (corrected 13 November 2026)

An earlier version of this section recommended returning `[]` / `[x]` instead of a `yield` iterator. That came from ad-hoc measurements whose wrapper methods stopped the JIT inlining the loop. In a real, inlined `foreach`, the .NET 11 JIT stack-allocates a `yield` iterator, while a collection returned through a delegate escapes to the heap. BenchmarkDotNet, each benchmark run on its own:

| `foreach` over… | `[]` / `[x]` | **`yield`** | v1 |
|---|---:|---:|---:|
| `maybe` (`Maybe<T>.GetEnumerator`) | 5.5 ns / 24 B | **0.29 ns / 0 B** | 0.57 ns / 0 B |
| `maybe.Unsafe()` (`Unsafe.GetEnumerator`) | 5.5 ns / 24 B | **2.6 ns / 0 B**, then ~0 ns after the `TryGetRight` change | 0.48 ns / 0 B |

Both enumerators use `yield`, with a code comment saying why. The enumerable helpers don't enumerate at all: they match each item with `is Some<T>(var x)` / `is Ok<T>(var x)`.

### `Result<T>` stores its failure in a box, and `Bind` / `Map` share it (13 November 2026)

Holding the 40-byte `Failure` inline made `Result<int>` 48 bytes, and copying it on every call made most Result operations slower than v1. `Result<T>` now holds a private `StrongBox<Failure>`, which brings it to about 16 bytes. The Ok path doesn't allocate, and a failure costs one 56 B box. Making `Failure` a record class was considered and rejected, because it would change the public type.

`Bind` and `Map` pass a failure along with the internal `PassFailure<TReturn>()`, which reuses the incoming box. Without it, each step would box the failure again: measured at 7.3 ns on the Ok path, and 15 ns plus 56 B per step on the failure path. `PassFailure` is marked `AggressiveInlining`, because without the hint the JIT won't optimise `Bind`'s Ok path (6.9 ns instead of 2.1 ns).

Both use a discard (`_ =>`) for the failure arm, with a code comment saying why. An explicit `Failure =>` arm makes the JIT set up a 40-byte `Failure` local on every call, including the Ok path. That costs about 0.5–1 ns per call, but it compounds across a chain: Result `Chain` (`Map`, then `Bind`, then `Match`) measured 28.8 ns with the explicit arm and 17.7 ns with the discard. Elsewhere, explicit case arms remain the default for readability.

`Map`, `Bind`, `Match`, `Filter` and LINQ `SelectMany` now use direct `switch` bodies instead of `M.Match` / `R.Match` with lambdas. The other combinators that still capture a lambda are listed under "Still to do" in section 0. Closures made Maybe `Map` allocate on every call and Maybe `Bind` take 5–8 ns. The full v1 vs v2 results are in `apps/Benchmarks/README.md`.

### Failure logging removed

Sections 4.5 and 8 (Phase 2) planned to move failure logging into the `Failure` constructor, with a non-logging path for `R.Pass`. Instead, logging was removed: `F.Log`, the logging hooks in `F`, and the logging in `Audit`. `R.Pass` no longer exists. `LogLevel` remains, so a `Failure` can still carry a level (`Lvl`) for callers to log themselves.

---

## Appendix: prototype evidence (SDK 11.0.100, Windows)

| Check | Result |
|---|---|
| `union Maybe<T>(None, Some<T>)` + body `Map`/`Bind` + custom `implicit operator(T)` | ✅ compiles and runs |
| Generic switch over that union with no `null` arm | ✅ no warning; **`default` throws `SwitchExpressionException`** |
| `union Either<L,R>(L,R)` used as `Either<string,int>` / `Either<int,int>` | ✅ works / ❌ CS0457 |
| LINQ query syntax via `Select`/`SelectMany` extensions on a union | ✅ |
| C# 14 `extension<T>(Result<T> r)` block on a union | ✅ |
| `a is IUnion` | ❌ CS8121: patterns test the *contents*. `((object)a) is IUnion` → true |
| Allocation: shorthand `Opt<int>` vs custom `[Union]` with `TryGetValue` | 24 B/op vs **0 B/op** |
| Custom `Maybe<T>` with `Some<T>`/`None` record-struct cases, unconstrained `T`, no null arm | ✅ no warning, `default` → None |
| Same, with `s.Value is not null` in the constructor | 0 B/op once optimised (`int`, `int?`, `string`); 24 B/op for `int` in tier-0 code only, with or without a local |
| `closed record Shape` with two sealed derived records, switch without `_` | ✅ exhaustive |
| STJ `Serialize`: shorthand Some / None / custom Some / custom None | `{"Value":5}` / `{}` / `5` / `{}` |
| STJ `Deserialize<Maybe<int>>` | ❌ "ambiguous for union type… specify a custom type classifier" |
| Union library on `net8.0` with internal `UnionAttribute`/`IUnion` polyfill, consumed by a net8 app on C# 15 / C# 12 | ✅ runs / ❌ CS9202 |

## Sources

- [Union types: C# language reference](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)
- [Unions: C# 15 feature specification](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-15.0/unions)
- [Explore union types in C# 15 (.NET Blog)](https://devblogs.microsoft.com/dotnet/csharp-15-union-types/)
- [Announcing .NET 11 (.NET Blog)](https://devblogs.microsoft.com/dotnet/announcing-dotnet-11/)
- [.NET 11 ships with C# 15 unions (Visual Studio Magazine)](https://visualstudiomagazine.com/articles/2026/11/10/net-11-ships-with-csharp-15-unions-faster-tools-and-coreclr-for-maui.aspx)
