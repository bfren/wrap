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
		Maybe<T> maybe = null!;

		// Act
		void action() => match(maybe);

		// Assert
		Assert.Throws<NullMaybeException>(action);
	}

	protected async Task Test00_Async<T>(Func<Maybe<T>, Task> match)
	{
		// Arrange
		Maybe<T> maybe = null!;

		// Act
		Task action() => match(maybe);

		// Assert
		await Assert.ThrowsAsync<NullMaybeException>(action);
	}

	protected void Test01<T>(Action<Maybe<T>> match)
	{
		// Arrange
		var maybe = new InvalidMaybe<T>();

		// Act
		void action() => match(maybe);

		// Assert
		var ex = Assert.Throws<InvalidMaybeTypeException>(action);
	}

	protected async Task Test01_Async<T>(Func<Maybe<T>, Task> match)
	{
		// Arrange
		var maybe = new InvalidMaybe<T>();

		// Act
		Task action() => match(maybe);

		// Assert
		await Assert.ThrowsAsync<InvalidMaybeTypeException>(action);
	}
}
