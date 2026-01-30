// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Abstracts;

public class Match_Tests
{
	public abstract class Null_Result : Match_Tests
	{
		public abstract void Test00_Throws_NullResultException();
	}

	public abstract class Null_Result_Async : Match_Tests
	{
		public abstract Task Test00_Throws_NullResultException();
	}

	public abstract class Unknown_Result : Match_Tests
	{
		public abstract void Test01_Throws_InvalidResultTypeException();
	}

	public abstract class Unknown_Result_Async : Match_Tests
	{
		public abstract Task Test01_Throws_InvalidResultTypeException();
	}

	protected static void Test00<T>(Action<Result<T>> match)
	{
		// Arrange
		Result<T> value = null!;

		// Act
		var result = Record.Exception(() => match(value));

		// Assert
		_ = Assert.IsType<NullResultException>(result);
	}

	protected async Task Test00_Async<T>(Func<Result<T>, Task> match)
	{
		// Arrange
		Result<T> value = null!;

		// Act
		var result = await Record.ExceptionAsync(() => match(value));

		// Assert
		_ = Assert.IsType<NullResultException>(result);
	}

	protected void Test01<T>(Action<Result<T>> match)
	{
		// Arrange
		var value = new InvalidResult<T>();

		// Act
		var result = Record.Exception(() => match(value));

		// Assert
		_ = Assert.IsType<InvalidResultTypeException>(result);
	}

	protected async Task Test01_Async<T>(Func<Result<T>, Task> match)
	{
		// Arrange
		var value = new InvalidResult<T>();

		// Act
		var result = await Record.ExceptionAsync(() => match(value));

		// Assert
		_ = Assert.IsType<InvalidResultTypeException>(result);
	}
}
