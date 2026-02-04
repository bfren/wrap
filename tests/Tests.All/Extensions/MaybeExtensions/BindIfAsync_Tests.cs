// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class BindIfAsync_Tests
{
	private static (Func<string, bool> predicate, Func<string, Maybe<int>> bind) Setup(bool predicateReturn)
	{
		var predicate = Substitute.For<Func<string, bool>>();
		predicate.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Maybe<int>>>();

		return (predicate, bind);
	}

	public class With_None
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (predicate, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r1 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r2 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				var r3 = await input.AsTask().BindIfAsync(predicate, bind);
				var r4 = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r5 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r6 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
				r3.AssertNone();
				r4.AssertNone();
				r5.AssertNone();
				r6.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (predicate, bind) = Setup(false);

				// Act
				_ = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(predicate, bind);
				_ = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (predicate, bind) = Setup(true);

				// Act
				var r0 = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r1 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r2 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				var r3 = await input.AsTask().BindIfAsync(predicate, bind);
				var r4 = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r5 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r6 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
				r3.AssertNone();
				r4.AssertNone();
				r5.AssertNone();
				r6.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (predicate, bind) = Setup(true);

				// Act
				_ = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(predicate, bind);
				_ = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_None()
			{
				var input = M.Wrap(Rnd.Str);
				var (predicate, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r1 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r2 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				var r3 = await input.AsTask().BindIfAsync(predicate, bind);
				var r4 = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r5 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r6 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
				r3.AssertNone();
				r4.AssertNone();
				r5.AssertNone();
				r6.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var (predicate, bind) = Setup(false);

				// Act
				_ = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(predicate, bind);
				_ = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				_ = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Some_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(Rnd.Str);
				var (predicate, bind) = Setup(true);
				bind.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r1 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r2 = await input.BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));
				var r3 = await input.AsTask().BindIfAsync(predicate, bind);
				var r4 = await input.AsTask().BindIfAsync(predicate, x => Task.FromResult(bind(x)));
				var r5 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), bind);
				var r6 = await input.AsTask().BindIfAsync(x => Task.FromResult(predicate(x)), x => Task.FromResult(bind(x)));

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
				r2.AssertSome(value);
				r3.AssertSome(value);
				r4.AssertSome(value);
				r5.AssertSome(value);
				r6.AssertSome(value);
			}
		}
	}
}
