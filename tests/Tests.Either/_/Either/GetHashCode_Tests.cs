// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Either_Tests;

public class GetHashCode_Tests
{
	[Fact]
	public void Same_Case_And_Value__Returns_Same_Hash_Code()
	{
		// Arrange
		var value = Rnd.Int;
		Either<string, int> v0 = new Right<int>(value);
		Either<string, int> v1 = new Right<int>(value);

		// Act
		var r0 = v0.GetHashCode();
		var r1 = v1.GetHashCode();

		// Assert
		Assert.Equal(r0, r1);
	}

	[Fact]
	public void Different_Case_Same_Value__Returns_Different_Hash_Code()
	{
		// Arrange
		var value = Rnd.Int;
		Either<int, int> v0 = new Left<int>(value);
		Either<int, int> v1 = new Right<int>(value);

		// Act
		var r0 = v0.GetHashCode();
		var r1 = v1.GetHashCode();

		// Assert
		Assert.NotEqual(r0, r1);
	}
}
