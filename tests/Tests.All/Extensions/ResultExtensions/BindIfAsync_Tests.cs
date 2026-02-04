// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class BindIfAsync_Tests
{
	private static (Func<string, bool> predicate, Func<string, Result<int>> bind) Setup(bool predicateReturn)
	{
		var predicate = Substitute.For<Func<string, bool>>();
		predicate.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Result<int>>>();

		return (predicate, bind);
	}

	public class With_Failure
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Failure()
			{
				// Arrange
				var value = Rnd.Str;
				var input = FailGen.Create<string>(new(value));
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
				r0.AssertFailure(value);
				r1.AssertFailure(value);
				r2.AssertFailure(value);
				r3.AssertFailure(value);
				r4.AssertFailure(value);
				r5.AssertFailure(value);
				r6.AssertFailure(value);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
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
			public async Task Returns_Failure()
			{
				// Arrange
				var value = Rnd.Str;
				var input = FailGen.Create<string>(new(value));
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
				r0.AssertFailure(value);
				r1.AssertFailure(value);
				r2.AssertFailure(value);
				r3.AssertFailure(value);
				r4.AssertFailure(value);
				r5.AssertFailure(value);
				r6.AssertFailure(value);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
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
				var input = R.Wrap(Rnd.Str);
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
				r0.AssertFailure(C.PredicateFalseMessage);
				r1.AssertFailure(C.PredicateFalseMessage);
				r2.AssertFailure(C.PredicateFalseMessage);
				r3.AssertFailure(C.PredicateFalseMessage);
				r4.AssertFailure(C.PredicateFalseMessage);
				r5.AssertFailure(C.PredicateFalseMessage);
				r6.AssertFailure(C.PredicateFalseMessage);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
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
				var input = R.Wrap(Rnd.Str);
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
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
				r3.AssertOk(value);
				r4.AssertOk(value);
				r5.AssertOk(value);
				r6.AssertOk(value);
			}
		}
	}
}
