// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class EnumerableBenchmarks
{
	// half Some, half None / Failure, so both paths are measured
	private readonly List<Maybe<int>> maybes =
		[.. Enumerable.Range(0, 1_000).Select(x => x % 2 == 0 ? M.Wrap(x) : M.None)];

	private readonly List<Result<int>> results =
		[.. Enumerable.Range(0, 1_000).Select(x => x % 2 == 0 ? R.Wrap(x) : R.Fail("Odd number."))];

	[Benchmark]
	public int MaybeFilter() =>
		maybes.Filter(x => x > 100).Count();

	[Benchmark]
	public int MaybeMap() =>
		maybes.Map(x => x + 1).Count();

	[Benchmark]
	public int MaybeFilterMap() =>
		maybes.FilterMap(x => x > 100, x => x + 1).Count();

	[Benchmark]
	public void MaybeIterate() =>
		maybes.Iterate(_ => { });

	[Benchmark]
	public int ResultFilter() =>
		results.Filter(x => x > 100).Count();

	[Benchmark]
	public int ResultMap() =>
		results.Map(x => x + 1).Count();

	[Benchmark]
	public int ResultFilterMap() =>
		results.FilterMap(x => x > 100, x => x + 1).Count();

	[Benchmark]
	public void ResultIterate() =>
		results.Iterate(_ => { });
}
