// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ObjectExtensions_Tests;

public class Wrap_Tests
{
	public class Single_Value
	{
		[Fact]
		public void Wraps_Value()
		{
			// Arrange
			var value = Rnd.Int;

			// Act
			var result = value.Wrap();

			// Assert
			Assert.Equal(value, result.Value);
		}
	}

	public class Enumerable_Value
	{
		[Fact]
		public void Wraps_Each_Value()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			IEnumerable<int> list = [v0, v1, v2];

			// Act
			var result = list.Wrap();

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value),
				x => Assert.Equal(v2, x.Value)
			);
		}

		[Fact]
		public void Skips_Null_Items()
		{
			// Arrange
			var v0 = Rnd.Str;
			var v1 = Rnd.Str;
			IEnumerable<string?> list = [v0, null, v1, null];

			// Act
			var result = list.Wrap();

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0, x.Value),
				x => Assert.Equal(v1, x.Value)
			);
		}
	}
}
