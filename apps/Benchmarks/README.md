# Wrap benchmarks

Compares Wrap 1.x with the current 2.x code using [BenchmarkDotNet](https://benchmarkdotnet.org).

- `AppBenchmarkv1` runs against **Wrap 1.4.0 from NuGet**.
- `AppBenchmarkv2` runs against the **local code** in `src/`.

Both apps compile the same files from this folder, so every benchmark runs identical code against each version. Both target `net11.0`, so the runtime is the same too.

## Running

Run each benchmark **in its own process**, in Release, from Git Bash. Run it from `apps/AppBenchmarkv1` or `apps/AppBenchmarkv2`:

```bash
dotnet build -c Release
dotnet run -c Release --no-build -- --list flat | grep '^Benchmarks\.' | while read -r b; do
  dotnet run -c Release --no-build -- --filter "$b" | grep -E '^\| [A-Z]' | grep -v '^| Method'
done
```

To run a single benchmark:

```bash
dotnet run -c Release -- --filter "Benchmarks.ResultBenchmarks.Bind"
```

Run v1 and v2 back to back on the same machine, with nothing heavy running, and compare the two outputs. BenchmarkDotNet can't join results across the two apps.

## Allocation check

The `Allocations` workflow runs `Check-Allocations.ps1` on every push to a branch that changes `src/`. It runs each v2 benchmark in its own process with `--job short`, about 12 minutes for all of them, and fails if any benchmark allocates more than `allocations.json` allows, plus 8 B for measurement noise. Allocations are checked because, unlike timings, they repeat exactly from run to run. Timings on shared runners are too noisy to compare.

```bash
pwsh apps/Benchmarks/Check-Allocations.ps1          # check against allocations.json
pwsh apps/Benchmarks/Check-Allocations.ps1 -Update  # rewrite allocations.json after an intended change
```

A new or renamed benchmark fails the check until `allocations.json` is updated.

## Things that skew the results

- **Several benchmarks in one run.** BenchmarkDotNet 0.15.8 can't start a separate process for `net11.0`, so `Program.cs` always adds `--inProcess`. Benchmarks in the same run then share JIT state and profile data. A method such as `R.Match<int,int>` ends up compiled for whichever benchmark ran first. `MatchFailure` measured 13–17 ns in a combined run but 2.5 ns on its own. Allocations change too: `MaybeMap` allocated 112 B in a combined run but 11.83 KB on its own. That's why the loop above starts a new process for each benchmark. Drop `--inProcess` once a BenchmarkDotNet release supports .NET 11.
- **`DOTNET_TieredCompilation=0`** changes what gets allocated, not only the timings. `MaybeFilter` allocated 112 B with it and 21.16 KB without it, so don't use it to speed up a run.
- **Ad-hoc timing loops and wrapper methods.** A helper that wraps the code under test can stop the JIT inlining it, which hides optimisations such as stack-allocating a `yield` iterator. An early test like that favoured returning `[x]` from `GetEnumerator`. In a real benchmark it was 20× slower than `yield`. Measure with BenchmarkDotNet, as here.
- **`--job dry`** runs a single cold iteration. It's useful for checking that everything runs, but its timings and allocations mean nothing.
- **About 0 ns** means the JIT optimised the call away entirely. Read it as "free", not as an exact figure.
- Repeated runs of the same benchmark differ by up to about 0.5 ns, so treat smaller differences as noise.

## Results

Measured on 14 November 2026 on branch `108-switch-to-use-union-types` (commit `a312d6f`, plus `GetFailure()` returning a `ref readonly`). Each benchmark ran in its own process, v1 and then v2 back to back, so each pair saw the same machine state. Earlier runs on 13 November came out 5–10% faster for both versions on some rows, so compare the two versions within one run, not across runs.

```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26300.9974)
AMD Ryzen 7 3700X 3.60GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 11.0.100
  [Host] : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3
```

Time is the mean per operation. Allocated is per operation. The enumerable benchmarks process 1,000 items, half Some/Ok and half None/Failure.

### EnumerableBenchmarks

| Benchmark | v1 time | v1 allocated | v2 time | v2 allocated | v2 vs v1 |
|---|---:|---:|---:|---:|---|
| MaybeFilter | 28.51 µs | 130.54 KB | 7.637 µs | 21.16 KB | **3.7× faster** |
| MaybeMap | 17.42 µs | 89.95 KB | 6.213 µs | 11.83 KB | **2.8× faster** |
| MaybeFilterMap | 42.35 µs | 224.3 KB | 7.932 µs | 21.17 KB | **5.3× faster** |
| MaybeIterate | 7.927 µs | 31.25 KB | 1.118 µs | 0 B | **7.1× faster** |
| ResultFilter | 43.92 µs | 172.87 KB | 15.25 µs | 6.5 KB | **2.9× faster** |
| ResultMap | 36.45 µs | 175.89 KB | 18.15 µs | 128 B | **2.0× faster** |
| ResultFilterMap | 65.51 µs | 305.22 KB | 14.80 µs | 6.52 KB | **4.4× faster** |
| ResultIterate | 9.843 µs | 31.25 KB | 1.113 µs | 0 B | **8.8× faster** |

### MaybeBenchmarks

| Benchmark | v1 time | v1 allocated | v2 time | v2 allocated | v2 vs v1 |
|---|---:|---:|---:|---:|---|
| WrapInt | 4.789 ns | 24 B | 0.0 ns | 0 B | **free** (inlined away) |
| WrapString | 5.914 ns | 24 B | 1.737 ns | 0 B | **3.4× faster** |
| WrapNull | 0.8299 ns | 0 B | 0.0459 ns | 0 B | **free** (inlined away) |
| Map | 11.53 ns | 48 B | 1.516 ns | 0 B | **7.6× faster** |
| Bind | 6.684 ns | 24 B | 1.831 ns | 0 B | **3.7× faster** |
| MatchSome | 2.051 ns | 0 B | 1.318 ns | 0 B | **1.6× faster** |
| MatchNone | 1.480 ns | 0 B | 1.285 ns | 0 B | **1.2× faster** |
| Chain | 20.54 ns | 72 B | 3.747 ns | 0 B | **5.5× faster** |
| Linq | 43.06 ns | 248 B | 2.297 ns | 0 B | **18.7× faster** |
| AsyncMap | 42.71 ns | 248 B | 29.49 ns | 144 B | **1.4× faster** |
| UnsafeUnwrap | 0.8041 ns | 0 B | 0.0 ns | 0 B | **free** (inlined away) |
| UnsafeForeach | 0.4775 ns | 0 B | 0.0059 ns | 0 B | **free** (inlined away) |
| Foreach | 0.7033 ns | 0 B | 0.0044 ns | 0 B | **free** (inlined away) |
| If | 15.13 ns | 64 B | 3.629 ns | 0 B | **4.2× faster** |
| MapIf | 29.41 ns | 144 B | 2.154 ns | 0 B | **13.7× faster** |
| MatchIf | 11.20 ns | 40 B | 3.327 ns | 0 B | **3.4× faster** |
| ToResult | 84.74 ns | 272 B | 65.71 ns | 168 B | **1.3× faster** |
| UnwrapNone | 16.03 ns | 104 B | 0.9661 ns | 0 B | **16.6× faster** |

### ResultBenchmarks

| Benchmark | v1 time | v1 allocated | v2 time | v2 allocated | v2 vs v1 |
|---|---:|---:|---:|---:|---|
| WrapInt | 4.616 ns | 24 B | 0.0240 ns | 0 B | **free** (inlined away) |
| WrapString | 6.244 ns | 24 B | 3.194 ns | 0 B | **2.0× faster** |
| Fail | 13.13 ns | 72 B | 6.689 ns | 56 B | **2.0× faster** |
| Map | 26.59 ns | 144 B | 11.94 ns | 0 B | **2.2× faster** |
| Bind | 6.528 ns | 24 B | 2.599 ns | 0 B | **2.5× faster** |
| MatchOk | 2.091 ns | 0 B | 1.866 ns | 0 B | **1.1× faster** |
| MatchFailure | 1.673 ns | 0 B | 2.156 ns | 0 B | 1.3× slower |
| Chain | 32.56 ns | 168 B | 8.336 ns | 0 B | **3.9× faster** |
| Linq | 56.41 ns | 344 B | 9.164 ns | 0 B | **6.2× faster** |
| AsyncMap | 58.16 ns | 344 B | 35.65 ns | 160 B | **1.6× faster** |
| UnsafeUnwrap | 0.4720 ns | 0 B | 0.1828 ns | 0 B | **2.6× faster** |
| If | 13.89 ns | 64 B | 3.323 ns | 0 B | **4.2× faster** |
| MapIf | 39.78 ns | 232 B | 8.337 ns | 0 B | **4.8× faster** |
| MatchIf | 10.78 ns | 40 B | 3.712 ns | 0 B | **2.9× faster** |
| ToMaybe | 14.51 ns | 48 B | 1.159 ns | 0 B | **12.5× faster** |
| UnwrapFailure | 6.323 ns | 24 B | 2.114 ns | 0 B | **3.0× faster** |

### Notes

- **v2 is faster in 41 of 42 benchmarks**, and it never allocates more than v1. Most single operations allocate nothing.
- **`If`, `MapIf`, `MatchIf`, `ToMaybe` and `Unwrap` allocate nothing in v2.** In v1 each call allocated 24–232 B, mostly for the closures that captured the caller's functions. `ToResult` on `None` still allocates 168 B, because it builds a `Failure` with a message and context.
- The `Filter` and `FilterMap` gains most likely come from the earlier rewrite of the single-value `Filter` as a `switch` (commit `f0616c3`).
- **`MatchFailure` is about 0.5 ns slower**, because `Result<T>` is a struct: the method reads three fields into registers, where v1 read one reference. Both versions copy the 40-byte failure once to pass it to `fFail`. `MatchOk` and `MatchNone` are faster than v1.
- **Result `Bind`, `Map`, `Match`, `Unwrap` and `ToMaybe` use a discard (`_ =>`) for the failure arm**, with a code comment saying why. An explicit `Failure =>` arm made the JIT set up a 40-byte `Failure` local on every call: Result `Chain` took 28.9 ns with it, 17.3 ns once `Bind` and `Map` used a discard, and 8.4 ns once `Match` did too.
- The reasons behind these numbers (`StrongBox<Failure>`, `PassFailure`, `TryGetLeft`/`TryGetRight`, `yield`) are recorded in section 10 of `docs/union-types-analysis.md`.
