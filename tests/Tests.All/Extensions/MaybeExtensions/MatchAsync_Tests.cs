// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MatchAsync_Tests
{
	public class Without_Return_Value
	{
		public class With_None
		{
			[Fact]
			public async Task Calls_FNone()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fNone = Substitute.For<Action>();

				// Act
				await input.MatchAsync(fNone, Substitute.For<Func<int, Task>>());
				await input.MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Action<int>>());
				await input.MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());
				await input.AsTask().MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());

				// Assert
				fNone.Received(4).Invoke();
			}

			[Fact]
			public async Task Does_Not_Call_FSome()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fSome = Substitute.For<Action<int>>();

				// Act
				await input.MatchAsync(Substitute.For<Action>(), x => { fSome(x); return Task.CompletedTask; });
				await input.MatchAsync(Substitute.For<Func<Task>>(), fSome);
				await input.MatchAsync(Substitute.For<Func<Task>>(), x => { fSome(x); return Task.CompletedTask; });
				await input.AsTask().MatchAsync(Substitute.For<Func<Task>>(), x => { fSome(x); return Task.CompletedTask; });

				// Assert
				fSome.DidNotReceive().Invoke(Arg.Any<int>());
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Action<int>>();

				// Act
				await input.MatchAsync(Substitute.For<Action>(), x => { fSome(x); return Task.CompletedTask; });
				await input.MatchAsync(Substitute.For<Func<Task>>(), fSome);
				await input.MatchAsync(Substitute.For<Func<Task>>(), x => { fSome(x); return Task.CompletedTask; });
				await input.AsTask().MatchAsync(Substitute.For<Func<Task>>(), x => { fSome(x); return Task.CompletedTask; });

				// Assert
				fSome.Received(4).Invoke(value);
			}

			[Fact]
			public async Task Does_Not_Call_FNone()
			{
				// Arrange
				var input = M.Wrap(Rnd.Int);
				var fNone = Substitute.For<Action>();

				// Act
				await input.MatchAsync(fNone, Substitute.For<Func<int, Task>>());
				await input.MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Action<int>>());
				await input.MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());
				await input.AsTask().MatchAsync(() => { fNone(); return Task.CompletedTask; }, Substitute.For<Func<int, Task>>());

				// Assert
				fNone.DidNotReceive().Invoke();
			}
		}
	}

	public class With_Return_Value
	{
		public class With_None
		{
			[Fact]
			public async Task Calls_FNone()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fNone = Substitute.For<Func<string>>();
				fNone.Invoke().Returns(Rnd.Str);

				// Act
				_ = await input.MatchAsync(fNone, Substitute.For<Func<int, Task<string>>>());
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, string>>());
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, Task<string>>>());
				_ = await input.AsTask().MatchAsync(fNone, Substitute.For<Func<int, string>>());
				_ = await input.AsTask().MatchAsync(fNone, Substitute.For<Func<int, Task<string>>>());
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, string>>());
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, Task<string>>>());

				// Assert
				fNone.Received(7).Invoke();
			}

			[Fact]
			public async Task Returns_FNone_Value()
			{
				// Arrange
				Maybe<int> input = M.None;
				var value = Rnd.Str;
				var fNone = Substitute.For<Func<string>>();
				fNone.Invoke().Returns(value);

				// Act
				var r0 = await input.MatchAsync(fNone, Substitute.For<Func<int, Task<string>>>());
				var r1 = await input.MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, string>>());
				var r2 = await input.MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, Task<string>>>());
				var r3 = await input.AsTask().MatchAsync(fNone, Substitute.For<Func<int, string>>());
				var r4 = await input.AsTask().MatchAsync(fNone, Substitute.For<Func<int, Task<string>>>());
				var r5 = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, string>>());
				var r6 = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), Substitute.For<Func<int, Task<string>>>());

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
				Assert.Equal(value, r4);
				Assert.Equal(value, r5);
				Assert.Equal(value, r6);
			}

			[Fact]
			public async Task Does_Not_Call_FSome()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fNone = Substitute.For<Func<string>>();
				fNone.Invoke().Returns(Rnd.Str);
				var fSome = Substitute.For<Func<int, string>>();

				// Act
				_ = await input.MatchAsync(fNone, Substitute.For<Func<int, Task<string>>>());
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), fSome);
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(fNone, fSome);
				_ = await input.AsTask().MatchAsync(fNone, x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), fSome);
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), x => Task.FromResult(fSome(x)));

				// Assert
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Func<int, string>>();

				// Act
				_ = await input.MatchAsync(Substitute.For<Func<string>>(), x => Task.FromResult(fSome(x)));
				_ = await input.MatchAsync(Substitute.For<Func<Task<string>>>(), fSome);
				_ = await input.MatchAsync(Substitute.For<Func<Task<string>>>(), x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(Substitute.For<Func<string>>(), fSome);
				_ = await input.AsTask().MatchAsync(Substitute.For<Func<string>>(), x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(Substitute.For<Func<Task<string>>>(), fSome);
				_ = await input.AsTask().MatchAsync(Substitute.For<Func<Task<string>>>(), x => Task.FromResult(fSome(x)));

				// Assert
				fSome.Received(7).Invoke(value);
			}

			[Fact]
			public async Task Returns_FSome_Value()
			{
				// Arrange
				var input = M.Wrap(Rnd.Int);
				var value = Rnd.Str;
				var fSome = Substitute.For<Func<int, string>>();
				fSome.Invoke(default).ReturnsForAnyArgs(value);

				// Act
				var r0 = await input.MatchAsync(Substitute.For<Func<string>>(), x => Task.FromResult(fSome(x)));
				var r1 = await input.MatchAsync(Substitute.For<Func<Task<string>>>(), fSome);
				var r2 = await input.MatchAsync(Substitute.For<Func<Task<string>>>(), x => Task.FromResult(fSome(x)));
				var r3 = await input.AsTask().MatchAsync(Substitute.For<Func<string>>(), fSome);
				var r4 = await input.AsTask().MatchAsync(Substitute.For<Func<string>>(), x => Task.FromResult(fSome(x)));
				var r5 = await input.AsTask().MatchAsync(Substitute.For<Func<Task<string>>>(), fSome);
				var r6 = await input.AsTask().MatchAsync(Substitute.For<Func<Task<string>>>(), x => Task.FromResult(fSome(x)));

				// Assert
				Assert.Equal(value, r0);
				Assert.Equal(value, r1);
				Assert.Equal(value, r2);
				Assert.Equal(value, r3);
				Assert.Equal(value, r4);
				Assert.Equal(value, r5);
				Assert.Equal(value, r6);
			}

			[Fact]
			public async Task Does_Not_Call_FNone()
			{
				// Arrange
				var input = M.Wrap(Rnd.Int);
				var fNone = Substitute.For<Func<string>>();
				var fSome = Substitute.For<Func<int, string>>();
				fSome.Invoke(default).ReturnsForAnyArgs(Rnd.Str);

				// Act
				_ = await input.MatchAsync(fNone, x => Task.FromResult(fSome(x)));
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), fSome);
				_ = await input.MatchAsync(() => Task.FromResult(fNone()), x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(fNone, fSome);
				_ = await input.AsTask().MatchAsync(fNone, x => Task.FromResult(fSome(x)));
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), fSome);
				_ = await input.AsTask().MatchAsync(() => Task.FromResult(fNone()), x => Task.FromResult(fSome(x)));

				// Assert
				fNone.DidNotReceive().Invoke();
			}
		}
	}
}
