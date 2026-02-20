// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class MapAsync_Tests
{
	public class With_None
	{
		[Fact]
		public async Task Does_Not_Call_Map_Function()
		{
			// Arrange
			IEnumerable<Maybe<string>> list = [M.None, M.None, M.None];
			var map = Substitute.For<Func<string, Maybe<int>>>();

			// Act
			_ = await list.MapAsync(async x => map(x));
			_ = await Task.FromResult(list).MapAsync(map);
			_ = await Task.FromResult(list).MapAsync(async x => map(x));

			// Assert
			map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public async Task Returns_None_Values()
		{
			// Arrange
			IEnumerable<Maybe<string>> list = [M.None, M.None, M.None];
			var map = Substitute.For<Func<string, Maybe<int>>>();

			// Act
			var r0 = await list.MapAsync(async x => map(x));
			var r1 = await Task.FromResult(list).MapAsync(map);
			var r2 = await Task.FromResult(list).MapAsync(async x => map(x));

			// Assert
			Assert.Collection(r0,
				x => x.AssertNone(),
				x => x.AssertNone(),
				x => x.AssertNone()
			);
			Assert.Collection(r1,
				x => x.AssertNone(),
				x => x.AssertNone(),
				x => x.AssertNone()
			);
			Assert.Collection(r2,
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
			public async Task Returns_Values()
			{
				// Arrange
				var v0 = Rnd.Str;
				var v1 = Rnd.Str;
				var v2 = Rnd.Str;
				IEnumerable<Maybe<string>> list = [v0, v1, v2];
				static string map(string x) => x.ToLower(F.DefaultCulture);

				// Act
				var r0 = await list.MapAsync(async x => map(x));
				var r1 = await Task.FromResult(list).MapAsync(map);
				var r2 = await Task.FromResult(list).MapAsync(async x => map(x));

				// Assert
				Assert.Collection(r0,
					x => x.AssertSome(v0.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v1.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v2.ToLower(F.DefaultCulture))
				);
				Assert.Collection(r1,
					x => x.AssertSome(v0.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v1.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v2.ToLower(F.DefaultCulture))
				);
				Assert.Collection(r2,
					x => x.AssertSome(v0.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v1.ToLower(F.DefaultCulture)),
					x => x.AssertSome(v2.ToLower(F.DefaultCulture))
				);
			}
		}
	}
}
