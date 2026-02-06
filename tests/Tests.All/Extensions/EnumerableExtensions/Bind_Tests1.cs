// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Bind_Tests
{
	internal static void AssertFailures<T0, T1>(Result<T0>[] expected, IEnumerable<Result<T1>> actual) =>
		Assert.Collection(actual,
			x => Assert.Equal(expected[0].AssertFailure(), x.AssertFailure()),
			x => Assert.Equal(expected[1].AssertFailure(), x.AssertFailure()),
			x => Assert.Equal(expected[2].AssertFailure(), x.AssertFailure())
		);

	public class With_Failure
	{
		[Fact]
		public void Does_Not_Call_Bind_Function()
		{
			// Arrange
			var list = new[] { FailGen.Create<int>(), FailGen.Create<int>(), FailGen.Create<int>() };
			var bind = Substitute.For<Func<int, Result<string>>>();

			// Act
			_ = list.Bind(bind);

			// Assert
			bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public void Returns_Original_Failures()
		{
			// Arrange
			var list = new[] { FailGen.Create<int>(), FailGen.Create<int>(), FailGen.Create<int>() };
			var bind = Substitute.For<Func<int, Result<string>>>();

			// Act
			var result = list.Bind(bind);

			// Assert
			AssertFailures(list, result);
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Returns_Values()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			var list = new[] { R.Wrap(v0), R.Wrap(v1), R.Wrap(v2) };

			// Act
			var result = list.Bind(x => R.Wrap(x.ToString()));

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0.ToString(), x),
				x => Assert.Equal(v1.ToString(), x),
				x => Assert.Equal(v2.ToString(), x)
			);
		}
	}

	public partial class With_Mixed
	{
		[Fact]
		public void Returns_All_Values()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			var list = new[] { R.Wrap(v0), FailGen.Create(), R.Wrap(v1), FailGen.Create(), R.Wrap(v2) };

			// Act
			var result = list.Bind(x => R.Wrap(x.ToString()));

			// Assert
			Assert.Equal(5, result.Count());
		}
	}
}
