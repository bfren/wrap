// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class BindIfAsync_Tests
{
	private static (Func<string, bool> fTest, Func<string, Result<int>> bind) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Result<int>>>();

		return (fTest, bind);
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
				var (fTest, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertFailure(value);
				r1.AssertFailure(value);
				r2.AssertFailure(value);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
				var (fTest, bind) = Setup(false);

				// Act
				_ = await input.BindIfAsync(fTest, async x => bind(x));
				_ = await input.AsTask().BindIfAsync(fTest, bind);
				_ = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

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
				var (fTest, bind) = Setup(true);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertFailure(value);
				r1.AssertFailure(value);
				r2.AssertFailure(value);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = FailGen.Create<string>();
				var (fTest, bind) = Setup(true);

				// Act
				_ = await input.BindIfAsync(fTest, async x => bind(x));
				_ = await input.AsTask().BindIfAsync(fTest, bind);
				_ = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

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
				var (fTest, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertFailure(C.TestFalseMessage);
				r1.AssertFailure(C.TestFalseMessage);
				r2.AssertFailure(C.TestFalseMessage);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(false);

				// Act
				_ = await input.BindIfAsync(fTest, async x => bind(x));
				_ = await input.AsTask().BindIfAsync(fTest, bind);
				_ = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

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
				var (fTest, bind) = Setup(true);
				bind.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}
	}
}
