// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Match_Tests
{
	public class Void_Overload
	{
		public class With_Failure
		{
			[Fact]
			public void Calls_fFail()
			{
				// Arrange
				var input = FailGen.Create<int>();
				var fFail = Substitute.For<Action<FailureValue>>();
				var fOk = Substitute.For<Action<int>>();

				// Act
				input.Match(fFail, fOk);

				// Assert
				fFail.ReceivedWithAnyArgs(1).Invoke(Arg.Any<FailureValue>());
			}

			[Fact]
			public void Does_Not_Call_fOk()
			{
				// Arrange
				var input = FailGen.Create<int>();
				var fFail = Substitute.For<Action<FailureValue>>();
				var fOk = Substitute.For<Action<int>>();

				// Act
				input.Match(fFail, fOk);

				// Assert
				fOk.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class With_Ok
		{
			[Fact]
			public void Calls_fOk()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fFail = Substitute.For<Action<FailureValue>>();
				var fOk = Substitute.For<Action<int>>();

				// Act
				input.Match(fFail, fOk);

				// Assert
				fOk.Received(1).Invoke(value);
			}

			[Fact]
			public void Does_Not_Call_fFail()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fFail = Substitute.For<Action<FailureValue>>();
				var fOk = Substitute.For<Action<int>>();

				// Act
				input.Match(fFail, fOk);

				// Assert
				fFail.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
			}
		}
	}

	public class Value_Overload
	{
		public class With_Failure
		{
			[Fact]
			public void Returns_fFail_Result()
			{
				// Arrange
				var failReturn = Rnd.Str;
				var input = FailGen.Create<int>();
				Func<FailureValue, string> fFail = _ => failReturn;
				Func<int, string> fOk = _ => Rnd.Str;

				// Act
				var result = input.Match(fFail, fOk);

				// Assert
				Assert.Equal(failReturn, result);
			}
		}

		public class With_Ok
		{
			[Fact]
			public void Returns_fOk_Result()
			{
				// Arrange
				var okReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);
				Func<FailureValue, string> fFail = _ => Rnd.Str;
				Func<int, string> fOk = _ => okReturn;

				// Act
				var result = input.Match(fFail, fOk);

				// Assert
				Assert.Equal(okReturn, result);
			}
		}
	}
}
