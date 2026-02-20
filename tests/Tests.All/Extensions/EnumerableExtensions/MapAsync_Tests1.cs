// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class MapAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Does_Not_Call_Map_Function()
		{
			// Arrange
			IEnumerable<Result<string>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var map = Substitute.For<Func<string, int>>();

			// Act
			_ = await list.MapAsync(async x => map(x));
			_ = await Task.FromResult(list).MapAsync(map);
			_ = await Task.FromResult(list).MapAsync(async x => map(x));

			// Assert
			map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}

		[Fact]
		public async Task Returns_Failure_Values()
		{
			// Arrange
			IEnumerable<Result<string>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var map = Substitute.For<Func<string, int>>();

			// Act
			var r0 = await list.MapAsync(async x => map(x));
			var r1 = await Task.FromResult(list).MapAsync(map);
			var r2 = await Task.FromResult(list).MapAsync(async x => map(x));

			// Assert
			Map_Tests.AssertFailures([.. list], r0);
			Map_Tests.AssertFailures([.. list], r1);
			Map_Tests.AssertFailures([.. list], r2);
		}
	}

	public class With_Ok
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
				IEnumerable<Result<string>> list = [v0, v1, v2];
				static string map(string x) => x.ToLower(F.DefaultCulture);

				// Act
				var r0 = await list.MapAsync(async x => map(x));
				var r1 = await Task.FromResult(list).MapAsync(map);
				var r2 = await Task.FromResult(list).MapAsync(async x => map(x));

				// Assert
				Assert.Collection(r0,
					x => x.AssertOk(v0.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v1.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v2.ToLower(F.DefaultCulture))
				);
				Assert.Collection(r1,
					x => x.AssertOk(v0.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v1.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v2.ToLower(F.DefaultCulture))
				);
				Assert.Collection(r2,
					x => x.AssertOk(v0.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v1.ToLower(F.DefaultCulture)),
					x => x.AssertOk(v2.ToLower(F.DefaultCulture))
				);
			}
		}
	}
}
