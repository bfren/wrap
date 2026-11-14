// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Maybe_Tests;

public class Default_Tests
{
	[Fact]
	public void Is_None()
	{
		// Arrange
		var maybe = default(Maybe<int>);

		// Act
		var result = maybe switch
		{
			Some<int> =>
				false,

			None =>
				true
		};

		// Assert
		Assert.True(result);
		maybe.AssertNone();
		Assert.Equal(M.None, maybe);
	}

	[Fact]
	public void Match_Runs_None_Function()
	{
		// Arrange
		var maybe = default(Maybe<string>);
		var none = Substitute.For<Action>();
		var some = Substitute.For<Action<string>>();

		// Act
		M.Match(maybe, none, some);

		// Assert
		none.Received(1).Invoke();
		some.DidNotReceive().Invoke(Arg.Any<string>());
	}

	[Fact]
	public void Value_Is_None()
	{
		// Arrange
		var maybe = default(Maybe<int>);

		// Act
		var result = maybe.Value;

		// Assert
		Assert.IsType<None>(result);
	}
}
