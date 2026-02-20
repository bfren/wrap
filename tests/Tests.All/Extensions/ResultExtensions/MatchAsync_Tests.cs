// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MatchAsync_Tests
{
	public class Void_Overload
	{
		public class With_Failure
		{
			[Fact]
			public async Task Calls_fFail()
			{
				// Arrange
				var input = FailGen.Create<int>();
				var fFail = Substitute.For<Func<FailureValue, Task>>();
				var fOk = Substitute.For<Func<int, Task>>();

				// Act
				await input.MatchAsync(fFail, fOk);
				await input.AsTask().MatchAsync(fFail, fOk);

				// Assert
				await fFail.ReceivedWithAnyArgs(2).Invoke(Arg.Any<FailureValue>());
			}

			[Fact]
			public async Task Does_Not_Call_fOk()
			{
				// Arrange
				var input = FailGen.Create<int>();
				var fFail = Substitute.For<Func<FailureValue, Task>>();
				var fOk = Substitute.For<Func<int, Task>>();

				// Act
				await input.MatchAsync(fFail, fOk);
				await input.AsTask().MatchAsync(fFail, fOk);

				// Assert
				await fOk.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class With_Ok
		{
			[Fact]
			public async Task Calls_fOk()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fFail = Substitute.For<Func<FailureValue, Task>>();
				var fOk = Substitute.For<Func<int, Task>>();

				// Act
				await input.MatchAsync(fFail, fOk);
				await input.AsTask().MatchAsync(fFail, fOk);

				// Assert
				await fOk.Received(2).Invoke(value);
			}

			[Fact]
			public async Task Does_Not_Call_fFail()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fFail = Substitute.For<Func<FailureValue, Task>>();
				var fOk = Substitute.For<Func<int, Task>>();

				// Act
				await input.MatchAsync(fFail, fOk);
				await input.AsTask().MatchAsync(fFail, fOk);

				// Assert
				await fFail.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<FailureValue>());
			}
		}
	}

	public class Value_Overload
	{
		public class With_Failure
		{
			[Fact]
			public async Task Returns_fFail_Result()
			{
				// Arrange
				var failReturn = Rnd.Str;
				var input = FailGen.Create<int>();

				// Act
				var r0 = await input.MatchAsync(fFail: _ => failReturn, fOk: _ => Task.FromResult(Rnd.Str));
				var r1 = await input.AsTask().MatchAsync(fFail: _ => failReturn, fOk: _ => Rnd.Str);
				var r2 = await input.MatchAsync(fFail: _ => Task.FromResult(failReturn), fOk: _ => Rnd.Str);
				var r3 = await input.MatchAsync(fFail: _ => Task.FromResult(failReturn), fOk: _ => Task.FromResult(Rnd.Str));

				// Assert
				Assert.Equal(failReturn, r0);
				Assert.Equal(failReturn, r1);
				Assert.Equal(failReturn, r2);
				Assert.Equal(failReturn, r3);
			}
		}

		public class With_Ok
		{
			[Fact]
			public async Task Returns_fOk_Result()
			{
				// Arrange
				var okReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act
				var r0 = await input.MatchAsync(fFail: _ => Task.FromResult(Rnd.Str), fOk: _ => okReturn);
				var r1 = await input.AsTask().MatchAsync(fFail: _ => Rnd.Str, fOk: _ => okReturn);
				var r2 = await input.MatchAsync(fFail: _ => Rnd.Str, fOk: _ => Task.FromResult(okReturn));
				var r3 = await input.MatchAsync(fFail: _ => Task.FromResult(Rnd.Str), fOk: _ => Task.FromResult(okReturn));

				// Assert
				Assert.Equal(okReturn, r0);
				Assert.Equal(okReturn, r1);
				Assert.Equal(okReturn, r2);
				Assert.Equal(okReturn, r3);
			}
		}
	}
}
