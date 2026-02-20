// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ObjectExtensions_Tests;

public class WrapAsync_Tests
{
	public class Single_Value
	{
		[Fact]
		public async Task Wraps_Value()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			var r0 = await value.WrapAsync();
			var r1 = await Task.FromResult(value).WrapAsync();

			// Assert
			Assert.Equal(value, r0.Value);
			Assert.Equal(value, r1.Value);
		}
	}

	public class Enumerable_Value
	{
		[Fact]
		public async Task Wraps_Each_Value()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			IEnumerable<int> list = [v0, v1, v2];

			// Act
			var r0 = await list.WrapAsync();
			var r1 = await Task.FromResult(list).WrapAsync();

			// Assert
			Assert.Collection(r0,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value),
				x => Assert.Equal(v2, x.Value)
			);
			Assert.Collection(r1,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value),
				x => Assert.Equal(v2, x.Value)
			);
		}

		[Fact]
		public async Task Skips_Null_Items()
		{
			// Arrange
			var v0 = Rnd.Str;
			var v1 = Rnd.Str;
			IEnumerable<string?> list = [v0, null, v1, null];

			// Act
			var r0 = await list.WrapAsync();
			var r1 = await Task.FromResult(list).WrapAsync();

			// Assert
			Assert.Collection(r0,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value)
			);
			Assert.Collection(r1,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value)
			);
		}
	}
}
