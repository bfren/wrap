// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Map_Tests
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
		public void Does_Not_Call_Map_Function()
		{
			// Arrange
			var list = new[] { FailGen.Create<string>(), FailGen.Create<string>(), FailGen.Create<string>() };
			var map = Substitute.For<Func<string, int>>();

			// Act
			_ = list.Map(map);

			// Assert
			map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_Original_Failures()
		{
			// Arrange
			var list = new[] { FailGen.Create<string>(), FailGen.Create<string>(), FailGen.Create<string>() };
			var map = Substitute.For<Func<string, int>>();

			// Act
			var result = list.Map(map);

			// Assert
			AssertFailures([.. list], result);
		}
	}

	public class With_Ok
	{
		public class Func_Returns_Value
		{
			[Fact]
			public void Returns_Values()
			{
				// Arrange
				var v0 = Rnd.Str;
				var v1 = Rnd.Str;
				var v2 = Rnd.Str;
				var list = new[] { R.Wrap(v0), R.Wrap(v1), R.Wrap(v2) };

				// Act
				var result = list.Map(x => x.ToLower(F.DefaultCulture));

				// Assert
				Assert.Collection(result,
					x => Assert.Equal(v0.ToLower(F.DefaultCulture), x),
					x => Assert.Equal(v1.ToLower(F.DefaultCulture), x),
					x => Assert.Equal(v2.ToLower(F.DefaultCulture), x)
				);
			}
		}
	}
}
