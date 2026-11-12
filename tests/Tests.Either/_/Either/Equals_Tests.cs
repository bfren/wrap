// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class Equals_Tests
{
	public class Left
	{
		[Fact]
		public void Same_Value_Returns_True()
		{
			// Arrange
			var value = Rnd.Str;
			Either<string, int> v0 = new Left<string>(value);
			Either<string, int> v1 = new Left<string>(value);

			// Act
			var r0 = v0.Equals(v1);
			var r1 = v0 == v1;
			var r2 = v0 != v1;
			var r3 = v0.Equals((object)v1);

			// Assert
			Assert.True(r0);
			Assert.True(r1);
			Assert.False(r2);
			Assert.True(r3);
		}

		[Fact]
		public void Different_Value_Returns_False()
		{
			// Arrange
			Either<string, int> v0 = new Left<string>(Rnd.Str);
			Either<string, int> v1 = new Left<string>(Rnd.Str);

			// Act
			var r0 = v0.Equals(v1);
			var r1 = v0 == v1;
			var r2 = v0 != v1;

			// Assert
			Assert.False(r0);
			Assert.False(r1);
			Assert.True(r2);
		}
	}

	public class Right
	{
		[Fact]
		public void Same_Value_Returns_True()
		{
			// Arrange
			var value = Rnd.Int;
			Either<string, int> v0 = new Right<int>(value);
			Either<string, int> v1 = new Right<int>(value);

			// Act
			var r0 = v0.Equals(v1);
			var r1 = v0 == v1;
			var r2 = v0 != v1;

			// Assert
			Assert.True(r0);
			Assert.True(r1);
			Assert.False(r2);
		}

		[Fact]
		public void Different_Value_Returns_False()
		{
			// Arrange
			Either<string, int> v0 = new Right<int>(Rnd.Int);
			Either<string, int> v1 = new Right<int>(Rnd.Int);

			// Act
			var r0 = v0.Equals(v1);
			var r1 = v0 == v1;
			var r2 = v0 != v1;

			// Assert
			Assert.False(r0);
			Assert.False(r1);
			Assert.True(r2);
		}
	}

	public class Different_Cases
	{
		[Fact]
		public void Same_Value_Returns_False()
		{
			// Arrange
			var value = Rnd.Int;
			Either<int, int> v0 = new Left<int>(value);
			Either<int, int> v1 = new Right<int>(value);

			// Act
			var r0 = v0.Equals(v1);
			var r1 = v0 == v1;
			var r2 = v0 != v1;

			// Assert
			Assert.False(r0);
			Assert.False(r1);
			Assert.True(r2);
		}
	}

	public class Other_Type
	{
		[Fact]
		public void Returns_False()
		{
			// Arrange
			Either<string, int> either = new Right<int>(Rnd.Int);

			// Act
			var result = either.Equals(Rnd.Str);

			// Assert
			Assert.False(result);
		}
	}
}
