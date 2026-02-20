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
				var value = Rnd.Str;
				var input = FailGen.Create<int>();

				// Act
				var r0 = await input.MatchAsync(fFail: _ => value, fOk: async _ => Rnd.Str);
				var r1 = await input.AsTask().MatchAsync(fFail: _ => value, fOk: _ => Rnd.Str);
				var r2 = await input.MatchAsync(fFail: async _ => value, fOk: _ => Rnd.Str);
				var r3 = await input.MatchAsync(fFail: async _ => value, fOk: async _ => Rnd.Str);

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
			}
		}

		public class With_Ok
		{
			[Fact]
			public async Task Returns_fOk_Result()
			{
				// Arrange
				var value = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act
				var r0 = await input.MatchAsync(fFail: async _ => Rnd.Str, fOk: _ => value);
				var r1 = await input.AsTask().MatchAsync(fFail: _ => Rnd.Str, fOk: _ => value);
				var r2 = await input.MatchAsync(fFail: _ => Rnd.Str, fOk: async _ => value);
				var r3 = await input.MatchAsync(fFail: async _ => Rnd.Str, fOk: async _ => value);

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
			}
		}
	}
}
