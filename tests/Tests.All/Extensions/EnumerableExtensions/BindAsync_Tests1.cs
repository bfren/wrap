// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class BindAsync_Tests
{
	private static ResultVars SetupResult(bool withValues, bool mixed = false)
	{
		var values = new[] { Rnd.Int, Rnd.Int, Rnd.Int };
		return new(
			GetResult(withValues ? values : null, mixed),
			Substitute.For<Func<int, Result<string>>>(),
			values
		);
	}

	private static IEnumerable<Result<int>> GetResult(int[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return R.Wrap(values[i]);
			}
			else
			{
				yield return FailGen.Create();
			}

			if (mixed)
			{
				yield return FailGen.Create();
			}
		}
	}

	public class With_Failure
	{
		[Fact]
		public async Task Does_Not_Call_Bind_Function()
		{
			// Arrange
			var v = SetupResult(false);

			// Act
			_ = await v.List.BindAsync(x => Task.FromResult(v.Bind(x)));
			_ = await v.ListAsync.BindAsync(v.Bind);
			_ = await v.ListAsync.BindAsync(x => Task.FromResult(v.Bind(x)));

			// Assert
			v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public async Task Returns_Failures()
		{
			// Arrange
			var v = SetupResult(false);

			// Act
			var r0 = await v.List.BindAsync(Substitute.For<Func<int, Task<Result<string>>>>());
			var r1 = await v.ListAsync.BindAsync(Substitute.For<Func<int, Result<string>>>());
			var r2 = await v.ListAsync.BindAsync(Substitute.For<Func<int, Task<Result<string>>>>());

			// Assert
			Assert.Equal(3, r0.Count());
			Assert.Equal(3, r1.Count());
			Assert.Equal(3, r2.Count());
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Returns_Values()
		{
			// Arrange
			var v = SetupResult(true);

			// Act
			var r0 = await v.List.BindAsync(x => R.Wrap(x.ToString()).AsTask());
			var r1 = await v.ListAsync.BindAsync(x => R.Wrap(x.ToString()));
			var r2 = await v.ListAsync.BindAsync(x => R.Wrap(x.ToString()).AsTask());

			// Assert
			Assert.Collection(r0,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r1,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r2,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
		}
	}

	public partial class With_Mixed
	{
		[Fact]
		public async Task Returns_All_Values()
		{
			// Arrange
			var v = SetupResult(true, true);

			// Act
			var r0 = await v.List.BindAsync(x => R.Wrap(x.ToString()).AsTask());
			var r1 = await v.ListAsync.BindAsync(x => R.Wrap(x.ToString()));
			var r2 = await v.ListAsync.BindAsync(x => R.Wrap(x.ToString()).AsTask());

			// Assert
			Assert.Equal(6, r0.Count());
			Assert.Equal(6, r1.Count());
			Assert.Equal(6, r2.Count());
		}
	}
}

internal record struct ResultVars(
	IEnumerable<Result<int>> List,
	Func<int, Result<string>> Bind,
	int[] Values
)
{
	public readonly Task<IEnumerable<Result<int>>> ListAsync =>
		Task.FromResult(List);
}
