// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Map_Tests
{
	public class With_None
	{
		[Fact]
		public void Does_Not_Call_Map_Function()
		{
			// Arrange
			var list = new Maybe<string>[] { M.None, M.None, M.None };
			var map = Substitute.For<Func<string, Maybe<int>>>();

			// Act
			_ = list.Map(map);

			// Assert
			map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public void Returns_None_Values()
		{
			// Arrange
			var list = new Maybe<string>[] { M.None, M.None, M.None };
			var map = Substitute.For<Func<string, Maybe<int>>>();

			// Act
			var result = list.Map(map);

			// Assert
			Assert.Collection(result,
				x => x.AssertNone(),
				x => x.AssertNone(),
				x => x.AssertNone()
			);
		}
	}

	public class With_Some
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
				var list = new[] { M.Wrap(v0), M.Wrap(v1), M.Wrap(v2) };

				// Act
				var result = list.Map(x => M.Wrap(x.ToLower(F.DefaultCulture)));

				// Assert
				Assert.Collection(result,
					x => x.AssertSome(v0.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v1.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v2.ToLower(F.DefaultCulture))
				);
			}
		}
	}
}
