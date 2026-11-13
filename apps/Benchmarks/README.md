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

## Things that skew the results

- **Several benchmarks in one run.** BenchmarkDotNet 0.15.8 can't start a separate process for `net11.0`, so `Program.cs` always adds `--inProcess`. Benchmarks in the same run then share JIT state and profile data. A method such as `R.Match<int,int>` ends up compiled for whichever benchmark ran first. `MatchFailure` measured 13–17 ns in a combined run but 2.5 ns on its own. That's why the loop above starts a new process for each benchmark. Drop `--inProcess` once a BenchmarkDotNet release supports .NET 11.
- **Ad-hoc timing loops and wrapper methods.** A helper that wraps the code under test can stop the JIT inlining it, which hides optimisations such as stack-allocating a `yield` iterator. An early test like that favoured returning `[x]` from `GetEnumerator`. In a real benchmark it was 20× slower than `yield`. Measure with BenchmarkDotNet, as here.
- **`--job dry`** runs a single cold iteration. It's useful for checking that everything runs, but its timings mean nothing.
- **About 0 ns** means the JIT optimised the call away entirely. Read it as "free", not as an exact figure.
- Repeated runs of the same benchmark differ by up to about 0.5 ns, so treat smaller differences as noise.

## Results

Measured on 13 November 2026 on branch `108-switch-to-use-union-types`, each benchmark in its own process. The Result rows were re-measured after Result `Bind` and `Map` switched to a discard arm. The `Linq` rows were re-measured after `SelectMany` switched to direct `switch` bodies. The `Filter` and `FilterMap` rows were re-measured, and the `If`, `MapIf`, `MatchIf`, `ToResult`, `ToMaybe` and `Unwrap` rows added, after the remaining capturing lambdas in `src/All` were replaced with direct `switch` bodies. The other v2 rows were re-run then too and stayed within noise.

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
| MaybeFilter | 25.53 µs | 130.54 KB | 7.435 µs | 21.16 KB | **3.4× faster** |
| MaybeMap | 16.27 µs | 89.95 KB | 5.793 µs | 11.83 KB | **2.8× faster** |
| MaybeFilterMap | 36.41 µs | 224.3 KB | 8.084 µs | 21.17 KB | **4.5× faster** |
| MaybeIterate | 7.031 µs | 31.25 KB | 1.083 µs | 0 B | **6.5× faster** |
| ResultFilter | 40.02 µs | 172.87 KB | 15.24 µs | 6.5 KB | **2.6× faster** |
| ResultMap | 34.52 µs | 175.89 KB | 18.04 µs | 128 B | **1.9× faster** |
| ResultFilterMap | 59.63 µs | 305.22 KB | 14.99 µs | 6.52 KB | **4.0× faster** |
| ResultIterate | 9.317 µs | 31.25 KB | 1.134 µs | 0 B | **8.2× faster** |

### MaybeBenchmarks

| Benchmark | v1 time | v1 allocated | v2 time | v2 allocated | v2 vs v1 |
|---|---:|---:|---:|---:|---|
| WrapInt | 4.385 ns | 24 B | 0.0 ns | 0 B | **free** (inlined away) |
| WrapString | 5.414 ns | 24 B | 1.723 ns | 0 B | **3.1× faster** |
| WrapNull | 0.8261 ns | 0 B | 0.0167 ns | 0 B | **free** (inlined away) |
| Map | 10.37 ns | 48 B | 1.494 ns | 0 B | **6.9× faster** |
| Bind | 6.435 ns | 24 B | 1.804 ns | 0 B | **3.6× faster** |
| MatchSome | 1.505 ns | 0 B | 1.275 ns | 0 B | **1.2× faster** |
| MatchNone | 1.072 ns | 0 B | 1.268 ns | 0 B | 1.2× slower |
| Chain | 17.79 ns | 72 B | 3.806 ns | 0 B | **4.7× faster** |
| Linq | 38.56 ns | 248 B | 2.294 ns | 0 B | **16.8× faster** |
| AsyncMap | 39.43 ns | 248 B | 28.22 ns | 144 B | **1.4× faster** |
| UnsafeUnwrap | 0.4766 ns | 0 B | 0.0101 ns | 0 B | **free** (inlined away) |
| UnsafeForeach | 0.4789 ns | 0 B | 0.0072 ns | 0 B | **free** (inlined away) |
| Foreach | 0.5604 ns | 0 B | 0.2878 ns | 0 B | **1.9× faster** |
| If | 12.62 ns | 64 B | 3.635 ns | 0 B | **3.5× faster** |
| MapIf | 22.80 ns | 144 B | 2.031 ns | 0 B | **11.2× faster** |
| MatchIf | 8.572 ns | 40 B | 2.642 ns | 0 B | **3.2× faster** |
| ToResult | 88.62 ns | 272 B | 67.84 ns | 168 B | **1.3× faster** |
| UnwrapNone | 14.63 ns | 104 B | 0.9714 ns | 0 B | **15.1× faster** |

### ResultBenchmarks

| Benchmark | v1 time | v1 allocated | v2 time | v2 allocated | v2 vs v1 |
|---|---:|---:|---:|---:|---|
| WrapInt | 4.643 ns | 24 B | 0.3075 ns | 0 B | **15.1× faster** |
| WrapString | 5.982 ns | 24 B | 3.170 ns | 0 B | **1.9× faster** |
| Fail | 10.55 ns | 72 B | 6.258 ns | 56 B | **1.7× faster** |
| Map | 26.11 ns | 144 B | 12.44 ns | 0 B | **2.1× faster** |
| Bind | 6.340 ns | 24 B | 2.398 ns | 0 B | **2.6× faster** |
| MatchOk | 1.936 ns | 0 B | 2.757 ns | 0 B | 1.4× slower |
| MatchFailure | 1.673 ns | 0 B | 2.164 ns | 0 B | 1.3× slower |
| Chain | 35.50 ns | 168 B | 17.27 ns | 0 B | **2.1× faster** |
| Linq | 49.27 ns | 344 B | 9.015 ns | 0 B | **5.5× faster** |
| AsyncMap | 50.48 ns | 344 B | 35.09 ns | 160 B | **1.4× faster** |
| UnsafeUnwrap | 0.4630 ns | 0 B | 0.1846 ns | 0 B | **2.5× faster** |
| If | 14.79 ns | 64 B | 3.364 ns | 0 B | **4.4× faster** |
| MapIf | 34.21 ns | 232 B | 8.039 ns | 0 B | **4.3× faster** |
| MatchIf | 10.18 ns | 40 B | 3.412 ns | 0 B | **3.0× faster** |
| ToMaybe | 13.09 ns | 48 B | 2.827 ns | 0 B | **4.6× faster** |
| UnwrapFailure | 5.728 ns | 24 B | 1.942 ns | 0 B | **2.9× faster** |

### Notes

- **v2 is faster in 39 of 42 benchmarks**, and it never allocates more than v1. Most single operations allocate nothing.
- **`If`, `MapIf`, `MatchIf`, `ToMaybe` and `Unwrap` allocate nothing in v2.** In v1 each call allocated 24–232 B, mostly for the closures that captured the caller's functions. `ToResult` on `None` still allocates 168 B, because it builds a `Failure` with a message and context.
- The `Filter` and `FilterMap` gains most likely come from the earlier rewrite of the single-value `Filter` as a `switch` (commit `f0616c3`). These rows had not been re-measured since.
- **`Match` is a few tenths of a nanosecond slower** (`MatchNone`, `MatchOk`, `MatchFailure`). That is close to the noise level: `MatchOk` measured 2.2 ns and 2.8 ns in two runs of the same code.
- **Result `Bind` and `Map` use a discard (`_ =>`) for the failure arm**, with a code comment saying why. An explicit `Failure =>` arm made the JIT set up a 40-byte `Failure` local on every call: Result `Chain` took 28.9 ns with it and 17.3 ns without.
- The reasons behind these numbers (`StrongBox<Failure>`, `PassFailure`, `TryGetLeft`/`TryGetRight`, `yield`) are recorded in section 10 of `docs/union-types-analysis.md`.
