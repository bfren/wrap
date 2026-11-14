// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Match_Tests
{
	public class Without_Return_Value
	{
		public class Failure
		{
			[Fact]
			public void Executes_Failure_Action()
			{
				// Arrange
				var failure = FailGen.Create();
				var fail = Substitute.For<Action<Wrap.Failure>>();
				var ok = Substitute.For<Action<int>>();

				// Act
				R.Match(failure, fail, ok);

				// Assert
				fail.Received(1).Invoke(failure);
				ok.DidNotReceive().Invoke(Arg.Any<int>());
			}
		}

		public class Ok
		{
			[Fact]
			public void Executes_Ok_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var wrapped = R.Wrap(value);
				var fail = Substitute.For<Action<Wrap.Failure>>();
				var ok = Substitute.For<Action<int>>();

				// Act
				R.Match(wrapped, fail, ok);

				// Assert
				ok.Received(1).Invoke(value);
				fail.DidNotReceive().Invoke(Arg.Any<Wrap.Failure>());
			}
		}
	}
}
