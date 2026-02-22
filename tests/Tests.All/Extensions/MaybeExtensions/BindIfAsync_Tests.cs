// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class BindIfAsync_Tests
{
	private static (Func<string, bool> fTest, Func<string, Maybe<int>> bind) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<string, Maybe<int>>>();

		return (fTest, bind);
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
				var (fTest, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
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
			public async Task Returns_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var (fTest, bind) = Setup(true);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = NoneGen.Create<string>();
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
				var input = M.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(false);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
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
				var input = M.Wrap(Rnd.Str);
				var (fTest, bind) = Setup(true);
				bind.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.BindIfAsync(fTest, async x => bind(x));
				var r1 = await input.AsTask().BindIfAsync(fTest, bind);
				var r2 = await input.AsTask().BindIfAsync(fTest, async x => bind(x));

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
				r2.AssertSome(value);
			}
		}
	}
}
