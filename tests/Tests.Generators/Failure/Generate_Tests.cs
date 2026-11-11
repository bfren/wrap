// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Generators.FailureValueGenerator_Tests;

public class Generate_Tests
{
	[Fact]
	public void Without_Level__Uses_Default_Failure_Level()
	{
		// Arrange

		// Act
		var result = Failures.NoLevel(Rnd.Int);

		// Assert
		Assert.Equal(LogLevel.Warning, result.Level);
	}

	[Fact]
	public void With_Level__Uses_Level()
	{
		// Arrange

		// Act
		var result = Failures.Escaped(Rnd.Str);

		// Assert
		Assert.Equal(LogLevel.Error, result.Level);
	}

	[Fact]
	public void Keeps_Message_Template_And_Passes_Args()
	{
		// Arrange
		var userId = Rnd.Int;

		// Act
		var result = Failures.NoLevel(userId);

		// Assert
		Assert.Equal("User {UserId} was not found.", result.Message);
		Assert.Equal([userId], result.Args);
	}

	[Fact]
	public void Escapes_Quotes_And_Backslashes_In_Message()
	{
		// Arrange

		// Act
		var result = Failures.Escaped(Rnd.Str);

		// Assert
		Assert.Equal("Value \"{Value}\" is not a C:\\ path.", result.Message);
	}

	[Fact]
	public void Sets_Context()
	{
		// Arrange

		// Act
		var result = Failures.InRecordStruct.Nested();

		// Assert
		Assert.Equal("InRecordStruct.Nested()", result.Context);
	}
}
