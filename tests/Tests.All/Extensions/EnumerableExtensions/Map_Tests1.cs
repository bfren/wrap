// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class Map_Tests1
{
	internal static void AssertFailures<T0, T1>(IEnumerable<Result<T1>> actual) =>
		Assert.Collection(actual,
			x => x.AssertFailure(C.NullValueFailureMessage, typeof(T0).Name),
			x => x.AssertFailure(C.NullValueFailureMessage, typeof(T0).Name),
			x => x.AssertFailure(C.NullValueFailureMessage, typeof(T0).Name)
		);

	public class With_Null_Input
	{
		[Fact]
		public void Does_Not_Call_Map_Function()
		{
			// Arrange
			var list = new string[] { null!, null!, null! };
			var map = Substitute.For<Func<string, Result<int>>>();

			// Act
			_ = list.Map(map);

			// Assert
			map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_NullValue_Failures()
		{
			// Arrange
			var list = new string[] { null!, null!, null! };
			var map = Substitute.For<Func<string, Result<int>>>();

			// Act
			var result = list.Map(map);

			// Assert
			AssertFailures<string, int>(result);
		}
	}

	public class With_Value_Input
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
				var list = new[] { v0, v1, v2 };

				// Act
				var result = list.Map(x => R.Wrap(x.ToLower(F.DefaultCulture)));

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
