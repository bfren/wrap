// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class MatchAsync_Tests
{
	public class Without_Return_Value
	{
		public class Null_Result : Abstracts.Match_Tests.Null_Result_Async
		{
			[Fact]
			public override async Task Test00_Throws_NullResultException()
			{
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Action<FailureValue>>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Action<FailureValue>>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Action<FailureValue>>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Func<int, Task>>())
				);
			}
		}

		public class Invalid_Result : Abstracts.Match_Tests.Unknown_Result_Async
		{
			[Fact]
			public override async Task Test01_Throws_InvalidResultTypeException()
			{
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Action<FailureValue>>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Action<FailureValue>>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Action<FailureValue>>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task>>(), Substitute.For<Func<int, Task>>())
				);
			}
		}

		public class Failure
		{
			[Fact]
			public async Task Executes_Failure_Action()
			{
				// Arrange
				var value = FailGen.Value;
				var failure = FailGen.Create<int>(value);
				var fail = Substitute.For<Action<FailureValue>>();

				// Act
				await R.MatchAsync(failure, fail, Substitute.For<Func<int, Task>>());
				await R.MatchAsync(failure, async f => fail(f), Substitute.For<Action<int>>());
				await R.MatchAsync(failure, async f => fail(f), Substitute.For<Func<int, Task>>());
				await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Action<int>>());
				await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Func<int, Task>>());
				await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Action<int>>());
				await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Func<int, Task>>());

				// Assert
				fail.Received(7).Invoke(value);
			}
		}

		public class Ok
		{
			[Fact]
			public async Task Executes_Ok_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var failure = R.Wrap(value);
				var ok = Substitute.For<Action<int>>();

				// Act
				await R.MatchAsync(failure, Substitute.For<Action<FailureValue>>(), async x => ok(x));
				await R.MatchAsync(failure, Substitute.For<Func<FailureValue, Task>>(), ok);
				await R.MatchAsync(failure, Substitute.For<Func<FailureValue, Task>>(), async x => ok(x));
				await R.MatchAsync(failure.AsTask(), Substitute.For<Action<FailureValue>>(), ok);
				await R.MatchAsync(failure.AsTask(), Substitute.For<Func<FailureValue, Task>>(), ok);
				await R.MatchAsync(failure.AsTask(), Substitute.For<Action<FailureValue>>(), async x => ok(x));
				await R.MatchAsync(failure.AsTask(), Substitute.For<Func<FailureValue, Task>>(), async x => ok(x));

				// Assert
				ok.Received(7).Invoke(value);
			}
		}
	}

	public class With_Return_Value
	{
		public class Null_Result : Abstracts.Match_Tests.Null_Result_Async
		{
			[Fact]
			public override async Task Test00_Throws_NullResultException()
			{
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
			}
		}
		public class Invalid_Result : Abstracts.Match_Tests.Unknown_Result_Async
		{
			[Fact]
			public override async Task Test01_Throws_InvalidResultTypeException()
			{
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(r, Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(r =>
					R.MatchAsync(Task.FromResult(r), Substitute.For<Func<FailureValue, Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
			}
		}

		public class Failure
		{
			[Fact]
			public async Task Executes_Failure_Action()
			{
				// Arrange
				var value = FailGen.Value;
				var failure = FailGen.Create<int>(value);
				var fail = Substitute.For<Func<FailureValue, string>>();

				// Act
				await R.MatchAsync(failure, fail, Substitute.For<Func<int, Task<string>>>());
				await R.MatchAsync(failure, async f => fail(f), Substitute.For<Func<int, string>>());
				await R.MatchAsync(failure, async f => fail(f), Substitute.For<Func<int, Task<string>>>());
				await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Func<int, string>>());
				await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Func<int, Task<string>>>());
				await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Func<int, string>>());
				await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Func<int, Task<string>>>());

				// Assert
				fail.Received(7).Invoke(value);
			}

			[Fact]
			public async Task Returns_Failure_Value()
			{
				// Arrange
				var failure = FailGen.Create<int>();
				var value = Rnd.Str;
				var fail = Substitute.For<Func<FailureValue, string>>();
				fail.Invoke(Arg.Any<FailureValue>()).Returns(value);

				// Act
				var v0 = await R.MatchAsync(failure, fail, Substitute.For<Func<int, Task<string>>>());
				var v1 = await R.MatchAsync(failure, async f => fail(f), Substitute.For<Func<int, string>>());
				var v2 = await R.MatchAsync(failure, async f => fail(f), Substitute.For<Func<int, Task<string>>>());
				var v3 = await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Func<int, string>>());
				var v4 = await R.MatchAsync(failure.AsTask(), fail, Substitute.For<Func<int, Task<string>>>());
				var v5 = await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Func<int, string>>());
				var v6 = await R.MatchAsync(failure.AsTask(), async f => fail(f), Substitute.For<Func<int, Task<string>>>());

				// Assert
				Assert.Equal(value, v0);
				Assert.Equal(value, v1);
				Assert.Equal(value, v2);
				Assert.Equal(value, v3);
				Assert.Equal(value, v4);
				Assert.Equal(value, v5);
				Assert.Equal(value, v6);
			}
		}

		public class Ok
		{
			[Fact]
			public async Task Executes_Ok_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var wrapped = R.Wrap(value);
				var ok = Substitute.For<Func<int, string>>();

				// Act
				_ = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, string>>(), async x => ok(x));
				_ = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, Task<string>>>(), ok);
				_ = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, Task<string>>>(), async x => ok(x));
				_ = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, string>>(), ok);
				_ = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, string>>(), async x => ok(x));
				_ = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, Task<string>>>(), ok);
				_ = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, Task<string>>>(), async x => ok(x));

				// Assert
				ok.Received(7).Invoke(value);
			}

			[Fact]
			public async Task Returns_Ok_Value()
			{
				// Arrange
				var wrapped = R.Wrap(Rnd.Int);
				var value = Rnd.Str;
				var ok = Substitute.For<Func<int, string>>();
				ok.Invoke(default).ReturnsForAnyArgs(value);

				// Act
				var v0 = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, string>>(), async x => ok(x));
				var v1 = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, Task<string>>>(), ok);
				var v2 = await R.MatchAsync(wrapped, Substitute.For<Func<FailureValue, Task<string>>>(), async x => ok(x));
				var v3 = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, string>>(), ok);
				var v4 = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, string>>(), async x => ok(x));
				var v5 = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, Task<string>>>(), ok);
				var v6 = await R.MatchAsync(wrapped.AsTask(), Substitute.For<Func<FailureValue, Task<string>>>(), async x => ok(x));

				// Assert
				Assert.Equal(value, v0);
				Assert.Equal(value, v1);
				Assert.Equal(value, v2);
				Assert.Equal(value, v3);
				Assert.Equal(value, v4);
				Assert.Equal(value, v5);
				Assert.Equal(value, v6);
			}
		}
	}
}
