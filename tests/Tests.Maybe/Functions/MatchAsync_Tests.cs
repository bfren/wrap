// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class MatchAsync_Tests
{
	public class Without_Return_Value
	{
		public class Null_Maybe : Abstracts.Match_Tests.Null_Maybe_Async
		{
			[Fact]
			public override async Task Test00_Throws_NullMaybeException()
			{
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Action>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task>>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task>>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Action>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Action>(), Substitute.For<Func<int, Task>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task>>(), Substitute.For<Action<int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task>>(), Substitute.For<Func<int, Task>>())
				);
			}
		}

		public class Invalid_Maybe : Abstracts.Match_Tests.Unknown_Maybe_Async
		{
			[Fact]
			public override async Task Test01_Throws_InvalidMaybeTypeException()
			{
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Action>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task>>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task>>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Action>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Action>(), Substitute.For<Func<int, Task>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task>>(), Substitute.For<Action<int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task>>(), Substitute.For<Func<int, Task>>())
				);
			}
		}

		public class None
		{
			[Fact]
			public async Task Executes_None_Action()
			{
				// Arrange
				Maybe<int> maybe = M.None;
				var none = Substitute.For<Action>();

				// Act
				await M.MatchAsync(maybe, none, Substitute.For<Func<int, Task>>());
				await M.MatchAsync(maybe, () => { none(); return Task.CompletedTask; }, Substitute.For<Action<int>>());
				await M.MatchAsync(maybe, () => { none(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());
				await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Action<int>>());
				await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Func<int, Task>>());
				await M.MatchAsync(maybe.AsTask(), () => { none(); return Task.CompletedTask; }, Substitute.For<Action<int>>());
				await M.MatchAsync(maybe.AsTask(), () => { none(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());

				// Assert
				none.Received(7).Invoke();
			}
		}

		public class Some
		{
			[Fact]
			public async Task Executes_Some_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var maybe = M.Wrap(value);
				var some = Substitute.For<Action<int>>();

				// Act
				await M.MatchAsync(maybe, Substitute.For<Action>(), x => { some(x); return Task.CompletedTask; });
				await M.MatchAsync(maybe, Substitute.For<Func<Task>>(), some);
				await M.MatchAsync(maybe, Substitute.For<Func<Task>>(), x => { some(x); return Task.CompletedTask; });
				await M.MatchAsync(maybe.AsTask(), Substitute.For<Action>(), some);
				await M.MatchAsync(maybe.AsTask(), Substitute.For<Action>(), x => { some(x); return Task.CompletedTask; });
				await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task>>(), some);
				await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task>>(), x => { some(x); return Task.CompletedTask; });

				// Assert
				some.Received(7).Invoke(value);
			}
		}
	}

	public class With_Return_Value
	{
		public class Null_Maybe : Abstracts.Match_Tests.Null_Maybe_Async
		{
			[Fact]
			public override async Task Test00_Throws_NullMaybeException()
			{
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<int>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test00_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
			}
		}
		public class Invalid_Maybe : Abstracts.Match_Tests.Unknown_Maybe_Async
		{
			[Fact]
			public override async Task Test01_Throws_InvalidMaybeTypeException()
			{
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(m, Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<int>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<int>>(), Substitute.For<Func<int, Task<int>>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, int>>())
				);
				await Test01_Async<int>(m =>
					M.MatchAsync(Task.FromResult(m), Substitute.For<Func<Task<int>>>(), Substitute.For<Func<int, Task<int>>>())
				);
			}
		}

		public class None
		{
			[Fact]
			public async Task Executes_None_Action()
			{
				// Arrange
				Maybe<int> maybe = M.None;
				var none = Substitute.For<Func<string>>();

				// Act
				await M.MatchAsync(maybe, none, Substitute.For<Func<int, Task<string>>>());
				await M.MatchAsync(maybe, async () => none(), Substitute.For<Func<int, string>>());
				await M.MatchAsync(maybe, async () => none(), Substitute.For<Func<int, Task<string>>>());
				await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Func<int, string>>());
				await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Func<int, Task<string>>>());
				await M.MatchAsync(maybe.AsTask(), async () => none(), Substitute.For<Func<int, string>>());
				await M.MatchAsync(maybe.AsTask(), async () => none(), Substitute.For<Func<int, Task<string>>>());

				// Assert
				none.Received(7).Invoke();
			}

			[Fact]
			public async Task Returns_None_Value()
			{
				// Arrange
				Maybe<int> maybe = M.None;
				var value = Rnd.Str;
				var none = Substitute.For<Func<string>>();
				none.Invoke().Returns(value);

				// Act
				var v0 = await M.MatchAsync(maybe, none, Substitute.For<Func<int, Task<string>>>());
				var v1 = await M.MatchAsync(maybe, async () => none(), Substitute.For<Func<int, string>>());
				var v2 = await M.MatchAsync(maybe, async () => none(), Substitute.For<Func<int, Task<string>>>());
				var v3 = await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Func<int, string>>());
				var v4 = await M.MatchAsync(maybe.AsTask(), none, Substitute.For<Func<int, Task<string>>>());
				var v5 = await M.MatchAsync(maybe.AsTask(), async () => none(), Substitute.For<Func<int, string>>());
				var v6 = await M.MatchAsync(maybe.AsTask(), async () => none(), Substitute.For<Func<int, Task<string>>>());

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

		public class Some
		{
			[Fact]
			public async Task Executes_Some_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var maybe = M.Wrap(value);
				var some = Substitute.For<Func<int, string>>();

				// Act
				_ = await M.MatchAsync(maybe, Substitute.For<Func<string>>(), async x => some(x));
				_ = await M.MatchAsync(maybe, Substitute.For<Func<Task<string>>>(), some);
				_ = await M.MatchAsync(maybe, Substitute.For<Func<Task<string>>>(), async x => some(x));
				_ = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<string>>(), some);
				_ = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<string>>(), async x => some(x));
				_ = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task<string>>>(), some);
				_ = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task<string>>>(), async x => some(x));

				// Assert
				some.Received(7).Invoke(value);
			}

			[Fact]
			public async Task Returns_Some_Value()
			{
				// Arrange
				var maybe = M.Wrap(Rnd.Int);
				var value = Rnd.Str;
				var some = Substitute.For<Func<int, string>>();
				some.Invoke(default).ReturnsForAnyArgs(value);

				// Act
				var v0 = await M.MatchAsync(maybe, Substitute.For<Func<string>>(), async x => some(x));
				var v1 = await M.MatchAsync(maybe, Substitute.For<Func<Task<string>>>(), some);
				var v2 = await M.MatchAsync(maybe, Substitute.For<Func<Task<string>>>(), async x => some(x));
				var v3 = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<string>>(), some);
				var v4 = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<string>>(), async x => some(x));
				var v5 = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task<string>>>(), some);
				var v6 = await M.MatchAsync(maybe.AsTask(), Substitute.For<Func<Task<string>>>(), async x => some(x));

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
