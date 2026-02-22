// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfAsync_Tests
{
	private static (Func<string, bool> fTest, Func<string, Result<int>> fTrue, Func<string, Result<int>> fFalse) Setup(bool predicateReturn)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);
		var fTrue = Substitute.For<Func<string, Result<int>>>();
		var fFalse = Substitute.For<Func<string, Result<int>>>();
		return (fTest, fTrue, fFalse);
	}

	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var (fTest, fTrue, fFalse) = Setup(true);

			// Act
			var r0 = await input.IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));
			var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
			var r2 = await input.AsTask().IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_fFalse_Result()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(false);
				fFalse.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));
				var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
				var r2 = await input.AsTask().IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_fTrue_Result()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Str);
				var (fTest, fTrue, fFalse) = Setup(true);
				fTrue.Invoke(Arg.Any<string>()).Returns(value);

				// Act
				var r0 = await input.IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));
				var r1 = await input.AsTask().IfAsync(fTest, fTrue, fFalse);
				var r2 = await input.AsTask().IfAsync(fTest, async x => fTrue(x), async x => fFalse(x));

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}
	}

	// Act-if-true special case
	public class Act_If_True
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(false);
				var fThen = Substitute.For<Func<int, Task<Result<int>>>>();

				// Act
				var r0 = await input.IfAsync(fTest, fThen);
				var r1 = await input.AsTask().IfAsync(fTest, x => fThen(x));
				var r2 = await input.AsTask().IfAsync(fTest, fThen);

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_fThen_Result()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Task<Result<int>>>>();
				fThen.Invoke(Arg.Any<int>()).Returns(Task.FromResult<Result<int>>(value));

				// Act
				var r0 = await input.IfAsync(fTest, fThen);
				var r1 = await input.AsTask().IfAsync(fTest, x => fThen(x));
				var r2 = await input.AsTask().IfAsync(fTest, fThen);

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}
	}
}
