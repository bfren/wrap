// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Monad_Tests;

public class ToString_Tests
{
	[Fact]
	public void Value_Not_Set__Does_Not_Throw()
	{
		// Arrange
		var monad = new Monad<string>();

		// Act
		var result = monad.ToString();

		// Assert
		Assert.Equal("Monad { Value =  }", result);
	}

	[Fact]
	public void Value_Set__Returns_Record_String_With_Value()
	{
		// Arrange
		var value = Rnd.Str;
		var monad = Monad<string>.Wrap(value);

		// Act
		var result = monad.ToString();

		// Assert
		Assert.Equal($"Monad {{ Value = {value} }}", result);
	}
}
