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
	public void Without_Kind__Uses_Unspecified()
	{
		// Arrange

		// Act
		var result = Failures.NoLevel(Rnd.Int);

		// Assert
		Assert.Equal(FailureKind.Unspecified, result.Kind);
	}

	[Fact]
	public void With_Kind__Uses_Kind()
	{
		// Arrange

		// Act
		var result = Failures.WithKind(Rnd.Int);

		// Assert
		Assert.Equal(FailureKind.NotFound, result.Kind);
	}

	[Fact]
	public void With_Custom_Kind_And_Level__Uses_Both()
	{
		// Arrange

		// Act
		var result = Failures.WithCustomKind();

		// Assert
		Assert.Equal(new FailureKind("Merlin.PayloadTooLarge"), result.Kind);
		Assert.Equal(LogLevel.Information, result.Level);
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
	public void In_Interface__Generates_Method()
	{
		// Arrange

		// Act
		var result = Failures.IInInterface.Nested();

		// Assert
		Assert.Equal("In interface.", result.Message);
		Assert.Equal("IInInterface.Nested()", result.Context);
	}

	[Fact]
	public void In_Union__Generates_Method()
	{
		// Arrange

		// Act
		var result = Failures.InUnion.Nested();

		// Assert
		Assert.Equal("In union.", result.Message);
		Assert.Equal("InUnion.Nested()", result.Context);
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
	public void Type_And_Method_Named_With_Keywords__Generates_Method()
	{
		// Arrange
		var id = Rnd.Int;

		// Act
		var result = Failures.@event.@lock(id);

		// Assert
		Assert.Equal([id], result.Args);
		Assert.Equal("event.lock()", result.Context);
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

	[Fact]
	public void Exception_Only__Sets_Exception_And_Error_Level()
	{
		// Arrange
		var ex = new InvalidOperationException(Rnd.Str);

		// Act
		var result = Failures.FromException(ex);

		// Assert
		Assert.Equal("From exception.", result.Message);
		Assert.Empty(result.Args);
		Assert.Same(ex, result.Exception);
		Assert.Equal(LogLevel.Error, result.Level);
		Assert.Equal("Failures.FromException()", result.Context);
	}

	[Fact]
	public void Exception_First__Is_Not_An_Arg()
	{
		// Arrange
		var (ex, path) = (new Exception(Rnd.Str), Rnd.Str);

		// Act
		var result = Failures.ExceptionFirst(ex, path);

		// Assert
		Assert.Equal([path], result.Args);
		Assert.Same(ex, result.Exception);
	}

	[Fact]
	public void Exception_Last__Is_Not_An_Arg()
	{
		// Arrange
		var (ex, path) = (new Exception(Rnd.Str), Rnd.Str);

		// Act
		var result = Failures.ExceptionLast(path, ex);

		// Assert
		Assert.Equal([path], result.Args);
		Assert.Same(ex, result.Exception);
	}

	[Fact]
	public void Exception_In_Middle__Is_Not_An_Arg()
	{
		// Arrange
		var (ex, path, attempt) = (new Exception(Rnd.Str), Rnd.Str, Rnd.Int);

		// Act
		var result = Failures.ExceptionInMiddle(path, ex, attempt);

		// Assert
		Assert.Equal([path, attempt], result.Args);
		Assert.Same(ex, result.Exception);
	}

	[Fact]
	public void Derived_Exception__Is_The_Exception()
	{
		// Arrange
		var ex = new InvalidOperationException(Rnd.Str);

		// Act
		var result = Failures.DerivedException(ex);

		// Assert
		Assert.Same(ex, result.Exception);
		Assert.Equal(LogLevel.Error, result.Level);
	}

	[Fact]
	public void Nullable_Exception_Null__Sets_Null_Exception()
	{
		// Arrange

		// Act
		var result = Failures.NullableException(null);

		// Assert
		Assert.Null(result.Exception);
		Assert.Equal(LogLevel.Error, result.Level);
	}

	[Fact]
	public void Exception_With_Explicit_Level__Uses_Level()
	{
		// Arrange
		var ex = new Exception(Rnd.Str);

		// Act
		var result = Failures.ExceptionWithLevel(ex);

		// Assert
		Assert.Same(ex, result.Exception);
		Assert.Equal(LogLevel.Information, result.Level);
	}

	[Fact]
	public void Constrained_Type_Parameter__Is_The_Exception()
	{
		// Arrange
		var ex = new ArgumentException(Rnd.Str);

		// Act
		var result = ConstrainedFailures<ArgumentException>.FromConstrained(ex);

		// Assert
		Assert.Same(ex, result.Exception);
		Assert.Equal(LogLevel.Error, result.Level);
	}

	[Fact]
	public void Exception_Failure__Matches_R_Fail_Apart_From_Context_And_Kind()
	{
		// Arrange
		var (ex, path) = (new Exception(Rnd.Str), Rnd.Str);

		// Act
		var generated = Failures.ExceptionFirst(ex, path);
		var manual = R.Fail(ex).WithMessage("Unable to read {Path}.", path);

		// Assert
		Assert.Equal(manual.Message, generated.Message);
		Assert.Equal(manual.Args, generated.Args);
		Assert.Equal(manual.Level, generated.Level);
		Assert.Same(manual.Exception, generated.Exception);
	}

	[Fact]
	public void Non_Exception_Parameter__Is_Still_An_Arg()
	{
		// Arrange
		var value = Rnd.Str;

		// Act
		var result = Failures.NotAnException(value);

		// Assert
		Assert.Equal([value], result.Args);
		Assert.Null(result.Exception);
		Assert.Equal(LogLevel.Warning, result.Level);
	}
}
