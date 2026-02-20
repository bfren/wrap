// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MatchIfAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_fFail_Result()
		{
			// Arrange
			var failReturn = Rnd.Str;
			var input = FailGen.Create<int>();

			// Act
			var r0 = await input.AsTask().MatchIfAsync(
				fFail: _ => Task.FromResult(failReturn),
				fTest: _ => Task.FromResult(true),
				fFalse: _ => Task.FromResult(Rnd.Str),
				fTrue: _ => Task.FromResult(Rnd.Str)
			);
			var r1 = await Task.FromResult(input).MatchIfAsync(
				fFail: _ => failReturn,
				fTest: _ => true,
				fFalse: _ => Rnd.Str,
				fTrue: _ => Rnd.Str
			);

			// Assert
			Assert.Equal(failReturn, r0);
			Assert.Equal(failReturn, r1);
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
				var falseReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act
				var r0 = await input.AsTask().MatchIfAsync(
					fFail: _ => Task.FromResult(Rnd.Str),
					fTest: _ => Task.FromResult(false),
					fFalse: _ => Task.FromResult(falseReturn),
					fTrue: _ => Task.FromResult(Rnd.Str)
				);
				var r1 = await Task.FromResult(input).MatchIfAsync(
					fFail: _ => Rnd.Str,
					fTest: _ => false,
					fFalse: _ => falseReturn,
					fTrue: _ => Rnd.Str
				);

				// Assert
				Assert.Equal(falseReturn, r0);
				Assert.Equal(falseReturn, r1);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_fTrue_Result()
			{
				// Arrange
				var trueReturn = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act
				var r0 = await input.AsTask().MatchIfAsync(
					fFail: _ => Task.FromResult(Rnd.Str),
					fTest: _ => Task.FromResult(true),
					fFalse: _ => Task.FromResult(Rnd.Str),
					fTrue: _ => Task.FromResult(trueReturn)
				);
				var r1 = await Task.FromResult(input).MatchIfAsync(
					fFail: _ => Rnd.Str,
					fTest: _ => true,
					fFalse: _ => Rnd.Str,
					fTrue: _ => trueReturn
				);

				// Assert
				Assert.Equal(trueReturn, r0);
				Assert.Equal(trueReturn, r1);
			}
		}
	}
}
