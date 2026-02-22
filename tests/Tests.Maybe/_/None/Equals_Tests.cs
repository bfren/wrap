// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.None_Tests;

public class Equals_Tests
{
	public class With_None
	{
		[Fact]
		public void Returns_True()
		{
			// Arrange
			var n0 = new None();
			var n1 = new None();

			// Act
			var r0 = n0.Equals(n1);
			var r1 = n0 == n1;
			var r2 = n0 != n1;

			// Assert
			Assert.True(r0);
			Assert.True(r1);
			Assert.False(r2);
		}
	}

	public class With_Object
	{
		[Fact]
		public void Returns_True()
		{
			// Arrange
			var n0 = new None();
			object n1 = new None();

			// Act
			var result = n0.Equals(n1);

			// Assert
			Assert.True(result);
		}
	}

	public class With_Null
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var none = new None();

			// Act
			var result = none.Equals(null);

			// Assert
			Assert.False(result);
		}
	}
}
