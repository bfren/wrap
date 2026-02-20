// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MapIfAsync_Tests
{
	private static (Func<string, bool> fTest, Func<string, int> f) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);
		var f = Substitute.For<Func<string, int>>();
		return (fTest, f);
	}

	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var (fTest, f) = Setup(true);

			// Act
			var r0 = await input.MapIfAsync(fTest, async x => f(x));
			var r1 = await input.AsTask().MapIfAsync(fTest, f);
			var r2 = await input.AsTask().MapIfAsync(fTest, async x => f(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}

		[Fact]
		public async Task f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var (fTest, f) = Setup(true);

			// Act
			_ = await input.MapIfAsync(fTest, async x => f(x));
			_ = await input.AsTask().MapIfAsync(fTest, f);
			_ = await input.AsTask().MapIfAsync(fTest, async x => f(x));

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(Rnd.Str);
				var (fTest, f) = Setup(false);

				// Act
				var r0 = await input.MapIfAsync(fTest, async x => f(x));
				var r1 = await input.AsTask().MapIfAsync(fTest, f);
				var r2 = await input.AsTask().MapIfAsync(fTest, async x => f(x));

				// Assert
				r0.AssertFailure(C.TestFalseMessage);
				r1.AssertFailure(C.TestFalseMessage);
				r2.AssertFailure(C.TestFalseMessage);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Ok_With_Mapped_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, f) = Setup(true);
				f.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.MapIfAsync(fTest, async x => f(x));
				var r1 = await input.AsTask().MapIfAsync(fTest, f);
				var r2 = await input.AsTask().MapIfAsync(fTest, async x => f(x));

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}
	}
}
