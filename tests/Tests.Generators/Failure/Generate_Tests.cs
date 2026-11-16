// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Generators.FailureGenerator_Tests;

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

	[Fact]
	public void Overloads__Each_Get_Their_Own_Message()
	{
		// Arrange

		// Act
		var byId = Failures.Overload(Rnd.Int);
		var byName = Failures.Overload(Rnd.Str);

		// Assert
		Assert.Equal("Item {Id} was not found.", byId.Message);
		Assert.Equal("Item {Name} was not found.", byName.Message);
	}

	[Fact]
	public void Single_Array_Parameter__Is_One_Arg()
	{
		// Arrange
		string[] names = [Rnd.Str, Rnd.Str, Rnd.Str];

		// Act
		var result = Failures.ArrayParameter(names);

		// Assert
		var arg = Assert.Single(result.Args);
		Assert.Same(names, arg);
	}

	[Fact]
	public void Parameters_Named_Message_And_Context__Are_Args()
	{
		// Arrange
		var (message, context) = (Rnd.Str, Rnd.Str);

		// Act
		var result = Failures.ParameterNames(message, context);

		// Assert
		Assert.Equal([message, context], result.Args);
		Assert.Equal("{Message} in {Context}.", result.Message);
		Assert.Equal("Failures.ParameterNames()", result.Context);
	}

	[Fact]
	public void Parameters_Named_With_Keywords__Are_Args()
	{
		// Arrange
		var (@class, @event) = (Rnd.Str, Rnd.Int);

		// Act
		var result = Failures.KeywordParameters(@class, @event);

		// Assert
		Assert.Equal([@class, @event], result.Args);
	}

	[Fact]
	public void In_Generic_Type__Generates_Method()
	{
		// Arrange
		var value = Rnd.Int;

		// Act
		var result = Failures.InGeneric<int>.Bad(value);

		// Assert
		Assert.Equal([value], result.Args);
		Assert.Equal("InGeneric.Bad()", result.Context);
	}

	[Fact]
	public void In_Global_Namespace__Generates_Method()
	{
		// Arrange

		// Act
		var result = GlobalFailures.Global();

		// Assert
		Assert.Equal("Global.", result.Message);
		Assert.Equal("GlobalFailures.Global()", result.Context);
	}
}
