// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfNotAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var fTest = Substitute.For<Func<int, bool>>();
			fTest.Invoke(Arg.Any<int>()).Returns(false);
			var fThen = Substitute.For<Func<int, Task<Result<int>>>>();

			// Act
			var r0 = await input.IfNotAsync(fTest, fThen);
			var r1 = await input.AsTask().IfNotAsync(fTest, x => fThen(x));
			var r2 = await input.AsTask().IfNotAsync(fTest, fThen);

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Original_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(value);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Task<Result<int>>>>();

				// Act
				var r0 = await input.IfNotAsync(fTest, fThen);
				var r1 = await input.AsTask().IfNotAsync(fTest, x => fThen(x));
				var r2 = await input.AsTask().IfNotAsync(fTest, fThen);

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}

			[Fact]
			public async Task fThen_Is_Not_Invoked()
			{
				// Arrange
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);
				var fThen = Substitute.For<Func<int, Task<Result<int>>>>();

				// Act
				_ = await input.IfNotAsync(fTest, fThen);
				_ = await input.AsTask().IfNotAsync(fTest, fThen);

				// Assert
				await fThen.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_fThen_Result()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(Rnd.Int);
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(false);
				var fThen = Substitute.For<Func<int, Task<Result<int>>>>();
				fThen.Invoke(Arg.Any<int>()).Returns(Task.FromResult<Result<int>>(value));

				// Act
				var r0 = await input.IfNotAsync(fTest, fThen);
				var r1 = await input.AsTask().IfNotAsync(fTest, x => fThen(x));
				var r2 = await input.AsTask().IfNotAsync(fTest, fThen);

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
				r2.AssertOk(value);
			}
		}
	}
}
