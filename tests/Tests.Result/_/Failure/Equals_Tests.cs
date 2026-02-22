// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Equals_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Same_Value_Returns_True()
		{
			// Arrange
			var value = Rnd.Str;
			var f0 = new Failure(value);
			var f1 = new Failure(value);

			// Act
			var r0 = f0.Equals(f1);
			var r1 = f0 == f1;
			var r2 = f0 != f1;

			// Assert
			Assert.True(r0);
			Assert.True(r1);
			Assert.False(r2);
		}

		[Fact]
		public void Different_Value_Returns_False()
		{
			// Arrange
			var f0 = new Failure(Rnd.Str);
			var f1 = new Failure(Rnd.Str);

			// Act
			var r0 = f0.Equals(f1);
			var r1 = f0 == f1;
			var r2 = f0 != f1;

			// Assert
			Assert.False(r0);
			Assert.False(r1);
			Assert.True(r2);
		}
	}

	public class With_Object
	{
		[Fact]
		public void Same_Value_Returns_True()
		{
			// Arrange
			var value = Rnd.Str;
			var f0 = new Failure(value);
			object f1 = new Failure(value);

			// Act
			var result = f0.Equals(f1);

			// Assert
			Assert.True(result);
		}
		[Fact]
		public void Different_Value_Returns_False()
		{
			// Arrange
			var f0 = new Failure(Rnd.Str);
			object f1 = new Failure(Rnd.Str);

			// Act
			var result = f0.Equals(f1);

			// Assert
			Assert.False(result);
		}
		[Fact]
		public void Different_Type_Returns_False()
		{
			// Arrange
			var f0 = new Failure(Rnd.Str);
			object obj = new();

			// Act
			var result = f0.Equals(obj);

			// Assert
			Assert.False(result);
		}
	}

	public class With_Null
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			var none = new Failure(Rnd.Str);

			// Act
			var result = none.Equals(null);

			// Assert
			Assert.False(result);
		}
	}
}
