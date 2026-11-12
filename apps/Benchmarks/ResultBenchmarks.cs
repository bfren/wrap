// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class ResultBenchmarks
{
	private readonly int number = 42;

	private readonly string text = "wrap";

	private readonly Result<int> ok = R.Wrap(42);

	private readonly Result<int> failure = R.Fail("Something went wrong.");

	[Benchmark]
	public Result<int> WrapInt() =>
		R.Wrap(number);

	[Benchmark]
	public Result<string> WrapString() =>
		R.Wrap(text);

	[Benchmark]
	public Result<int> Fail() =>
		R.Fail("Something went wrong.");

	[Benchmark]
	public Result<int> Map() =>
		ok.Map(x => x + 1);

	[Benchmark]
	public Result<int> Bind() =>
		ok.Bind(x => R.Wrap(x + 1));

	[Benchmark]
	public int MatchOk() =>
		ok.Match(fFail: _ => 0, fOk: x => x);

	[Benchmark]
	public int MatchFailure() =>
		failure.Match(fFail: _ => 0, fOk: x => x);

	[Benchmark]
	public int Chain() =>
		ok.Map(x => x + 1).Bind(x => R.Wrap(x * 2)).Match(fFail: _ => 0, fOk: x => x);

	[Benchmark]
	public Result<int> Linq() =>
		from x in ok
		from y in R.Wrap(x + 1)
		select x + y;

	[Benchmark]
	public Task<Result<int>> AsyncMap() =>
		ok.AsTask().MapAsync(x => x + 1);

	[Benchmark]
	public int UnsafeUnwrap() =>
		ok.Unsafe().Unwrap();
}
