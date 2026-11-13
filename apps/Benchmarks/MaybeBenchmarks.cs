// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class MaybeBenchmarks
{
	private readonly int number = 42;

	private readonly string text = "wrap";

	private readonly Maybe<int> some = M.Wrap(42);

	private readonly Maybe<int> none = M.None;

	[Benchmark]
	public Maybe<int> WrapInt() =>
		M.Wrap(number);

	[Benchmark]
	public Maybe<string> WrapString() =>
		M.Wrap(text);

	[Benchmark]
	public Maybe<string?> WrapNull() =>
		M.Wrap<string?>(null);

	[Benchmark]
	public Maybe<int> Map() =>
		some.Map(x => x + 1);

	[Benchmark]
	public Maybe<int> Bind() =>
		some.Bind(x => M.Wrap(x + 1));

	[Benchmark]
	public int MatchSome() =>
		some.Match(fNone: () => 0, fSome: x => x);

	[Benchmark]
	public int MatchNone() =>
		none.Match(fNone: () => 0, fSome: x => x);

	[Benchmark]
	public int Chain() =>
		some.Map(x => x + 1).Bind(x => M.Wrap(x * 2)).Match(fNone: () => 0, fSome: x => x);

	[Benchmark]
	public Maybe<int> Linq() =>
		from x in some
		from y in M.Wrap(x + 1)
		select x + y;

	[Benchmark]
	public Task<Maybe<int>> AsyncMap() =>
		some.AsTask().MapAsync(x => x + 1);

	[Benchmark]
	public int UnsafeUnwrap() =>
		some.Unsafe().Unwrap();

	[Benchmark]
	public int UnsafeForeach()
	{
		foreach (var x in some.Unsafe())
		{
			return x;
		}

		return 0;
	}

	[Benchmark]
	public int Foreach()
	{
		foreach (var x in some)
		{
			return x;
		}

		return 0;
	}
}
