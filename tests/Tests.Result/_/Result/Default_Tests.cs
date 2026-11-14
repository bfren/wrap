// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class Default_Tests
{
	[Fact]
	public void Is_Uninitialised_Failure()
	{
		// Arrange
		var result = default(Result<int>);

		// Act
		var isFailure = result switch
		{
			Ok<int> =>
				false,

			Failure =>
				true
		};

		// Assert
		Assert.True(isFailure);
		_ = result.AssertFailure("Uninitialised Failure");
	}

	[Fact]
	public void Match_Runs_Failure_Function()
	{
		// Arrange
		var result = default(Result<string>);
		var fail = Substitute.For<Action<Failure>>();
		var ok = Substitute.For<Action<string>>();

		// Act
		R.Match(result, fail, ok);

		// Assert
		fail.Received(1).Invoke(Arg.Is<Failure>(f => f.Message == "Uninitialised Failure"));
		ok.DidNotReceive().Invoke(Arg.Any<string>());
	}

	[Fact]
	public void Value_Is_Failure()
	{
		// Arrange
		var result = default(Result<int>);

		// Act
		var value = result.Value;

		// Assert
		_ = Assert.IsType<Failure>(value);
	}
}
