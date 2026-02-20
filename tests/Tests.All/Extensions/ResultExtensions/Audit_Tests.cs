// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Audit_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Runs_fFail()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fFail = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.Audit(fFail);

			// Assert
			fFail.ReceivedWithAnyArgs(1).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public void Does_Not_Run_fOk()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fOk = Substitute.For<Action<int>>();

			// Act
			_ = input.Audit(fFail: null, fOk: fOk);

			// Assert
			fOk.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));

			// Act
			var result = input.Audit(fFail: null, fOk: null);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void fFail_Throws__Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));

			// Act
			var result = input.Audit(fFail: _ => throw new Exception(Rnd.Str));

			// Assert
			result.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Does_Not_Run_fFail()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var fFail = Substitute.For<Action<FailureValue>>();

			// Act
			_ = input.Audit(fFail: fFail, fOk: null);

			// Assert
			fFail.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public void Runs_fOk()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var fOk = Substitute.For<Action<int>>();

			// Act
			_ = input.Audit(fOk);

			// Assert
			fOk.Received(1).Invoke(value);
		}

		[Fact]
		public void Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = input.Audit(fFail: null, fOk: null);

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public void fOk_Throws__Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = input.Audit(fFail: _ => throw new Exception(Rnd.Str));

			// Assert
			result.AssertOk(value);
		}
	}

	public class With_Either
	{
		[Fact]
		public void With_Failure__Runs_Either()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var either = Substitute.For<Action<Result<int>>>();

			// Act
			_ = input.Audit(either);

			// Assert
			either.ReceivedWithAnyArgs(1).Invoke(Arg.Any<Result<int>>());
		}

		[Fact]
		public void With_Ok__Runs_Either()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var either = Substitute.For<Action<Result<int>>>();

			// Act
			_ = input.Audit(either);

			// Assert
			either.ReceivedWithAnyArgs(1).Invoke(Arg.Any<Result<int>>());
		}

		[Fact]
		public void Returns_Original_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = input.Audit(fFail: _ => { });

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public void Either_Throws__Returns_Original()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = input.Audit(fFail: _ => throw new Exception(Rnd.Str));

			// Assert
			result.AssertOk(value);
		}
	}
}
