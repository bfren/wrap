// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Abstracts;

public class Match_Tests
{
	public abstract class Null_Maybe : Match_Tests
	{
		public abstract void Test00_Throws_NullMaybeException();
	}

	public abstract class Null_Maybe_Async : Match_Tests
	{
		public abstract Task Test00_Throws_NullMaybeException();
	}

	public abstract class Unknown_Maybe : Match_Tests
	{
		public abstract void Test01_Throws_InvalidMaybeTypeException();
	}

	public abstract class Unknown_Maybe_Async : Match_Tests
	{
		public abstract Task Test01_Throws_InvalidMaybeTypeException();
	}

	protected static void Test00<T>(Action<Maybe<T>> match)
	{
		// Arrange
		Maybe<T> value = null!;

		// Act
		var result = Record.Exception(() => match(value));

		// Assert
		_ = Assert.IsType<NullMaybeException>(result);
	}

	protected async Task Test00_Async<T>(Func<Maybe<T>, Task> match)
	{
		// Arrange
		Maybe<T> value = null!;

		// Act
		var result = await Record.ExceptionAsync(() => match(value));

		// Assert
		_ = Assert.IsType<NullResultException>(result);
	}

	protected void Test01<T>(Action<Maybe<T>> match)
	{
		// Arrange
		var value = new InvalidMaybe<T>();

		// Act
		var result = Record.Exception(() => match(value));

		// Assert
		_ = Assert.IsType<InvalidMaybeTypeException>(result);
	}

	protected async Task Test01_Async<T>(Func<Maybe<T>, Task> match)
	{
		// Arrange
		var value = new InvalidMaybe<T>();

		// Act
		var result = await Record.ExceptionAsync(() => match(value));

		// Assert
		_ = Assert.IsType<InvalidMaybeTypeException>(result);
	}
}
