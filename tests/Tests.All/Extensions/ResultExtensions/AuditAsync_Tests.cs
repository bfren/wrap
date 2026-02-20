// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class AuditAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Runs_fFail()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fFail = Substitute.For<Func<FailureValue, Task>>();

			// Act
			_ = await input.AuditAsync(fFail);
			_ = await input.AsTask().AuditAsync(fFail);

			// Assert
			await fFail.ReceivedWithAnyArgs(2).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Action_Overload__Runs_fFail()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fFail = Substitute.For<Action<FailureValue>>();

			// Act
			_ = await input.AsTask().AuditAsync(fFail);

			// Assert
			fFail.ReceivedWithAnyArgs(1).Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Action_Overload__Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));

			// Act
			var result = await input.AsTask().AuditAsync(fFail: null, fOk: (Action<int>?)null);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public async Task Does_Not_Run_fOk()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var fOk = Substitute.For<Func<int, Task>>();

			// Act
			_ = await input.AuditAsync(fFail: null, fOk: fOk);
			_ = await input.AsTask().AuditAsync(fFail: null, fOk: fOk);

			// Assert
			await fOk.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));

			// Act
			var r0 = await input.AuditAsync(fFail: null, fOk: null);
			var r1 = await input.AsTask().AuditAsync(fFail: null, fOk: null);

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
		}

		[Fact]
		public async Task fFail_Throws__Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));

			// Act
			var r0 = await input.AuditAsync(fFail: _ => throw new Exception(Rnd.Str));
			var r1 = await input.AsTask().AuditAsync(fFail: _ => throw new Exception(Rnd.Str));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Does_Not_Run_fFail()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var fFail = Substitute.For<Func<FailureValue, Task>>();

			// Act
			_ = await input.AuditAsync(fFail: fFail, fOk: null);
			_ = await input.AsTask().AuditAsync(fFail: fFail, fOk: null);

			// Assert
			await fFail.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
		}

		[Fact]
		public async Task Runs_fOk()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var fOk = Substitute.For<Func<int, Task>>();

			// Act
			_ = await input.AuditAsync(fOk);
			_ = await input.AsTask().AuditAsync(fOk);

			// Assert
			await fOk.Received(2).Invoke(value);
		}

		[Fact]
		public async Task Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.AuditAsync(fFail: null, fOk: null);
			var r1 = await input.AsTask().AuditAsync(fFail: null, fOk: null);

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
		}

		[Fact]
		public async Task fOk_Throws__Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.AuditAsync(fOk: _ => throw new Exception(Rnd.Str));
			var r1 = await input.AsTask().AuditAsync(fOk: _ => throw new Exception(Rnd.Str));

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
		}

		[Fact]
		public async Task Action_Overload__Runs_fOk()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var fOk = Substitute.For<Action<int>>();

			// Act
			_ = await input.AsTask().AuditAsync(fOk);

			// Assert
			fOk.Received(1).Invoke(value);
		}

		[Fact]
		public async Task Action_Overload__Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var result = await input.AsTask().AuditAsync(fFail: null, fOk: (Action<int>?)null);

			// Assert
			result.AssertOk(value);
		}
	}

	public class With_Either
	{
		[Fact]
		public async Task With_Failure__Runs_Either()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var either = Substitute.For<Func<Result<int>, Task>>();

			// Act
			_ = await input.AuditAsync(either);
			_ = await input.AsTask().AuditAsync(either);

			// Assert
			await either.ReceivedWithAnyArgs(2).Invoke(Arg.Any<Result<int>>());
		}

		[Fact]
		public async Task With_Ok__Runs_Either()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var either = Substitute.For<Func<Result<int>, Task>>();

			// Act
			_ = await input.AuditAsync(either);
			_ = await input.AsTask().AuditAsync(either);

			// Assert
			await either.ReceivedWithAnyArgs(2).Invoke(Arg.Any<Result<int>>());
		}

		[Fact]
		public async Task Returns_Original_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.AuditAsync(fFail: _ => Task.CompletedTask);
			var r1 = await input.AsTask().AuditAsync(fFail: _ => Task.CompletedTask);

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
		}

		[Fact]
		public async Task Either_Throws__Returns_Original()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = await input.AuditAsync(fFail: _ => throw new Exception(Rnd.Str));
			var r1 = await input.AsTask().AuditAsync(fFail: _ => throw new Exception(Rnd.Str));

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
		}
	}
}
