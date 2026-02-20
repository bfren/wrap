// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class MatchIfAsync_Tests
{
	// 30 async overloads grouped as:
	//   A (7):  Result<T>        source, sync   fFail
	//   B (7):  Task<Result<T>>  source, sync   fFail
	//   C (8):  Result<T>        source, async  fFail
	//   D (8):  Task<Result<T>>  source, async  fFail  [D8 = canonical]
	//
	// Within each group, fTest / fFalse / fTrue vary sync vs async:
	//   1: fTest=S  fFalse=A  fTrue=S     (A,B only - A doesn't have S/S/S)
	//   2: fTest=S  fFalse=S  fTrue=A
	//   3: fTest=S  fFalse=A  fTrue=A
	//   4: fTest=A  fFalse=S  fTrue=S
	//   5: fTest=A  fFalse=A  fTrue=S
	//   6: fTest=A  fFalse=S  fTrue=A
	//   7: fTest=A  fFalse=A  fTrue=A
	//   B1/C1/D1: fTest=S fFalse=S fTrue=S  (B1 = existing r1)
	//   D8: fTest=A fFalse=A fTrue=A         (D8 = existing r0 / canonical)

	public class With_Failure
	{
		[Fact]
		public async Task Returns_fFail_Result()
		{
			// Arrange
			var failReturn = Rnd.Str;
			var input = FailGen.Create<int>();

			// Act

			// Group A: Result<T> source, sync fFail
			var a1 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var a2 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var a3 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));
			var a4 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var a5 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var a6 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var a7 = await input.MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));

			// Group B: Task<Result<T>> source, sync fFail
			var b1 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b2 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var b3 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var b4 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));
			var b5 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b6 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var b7 = await input.AsTask().MatchIfAsync(fFail: _ => failReturn, fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));

			// Group C: Result<T> source, async fFail
			var c1 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c2 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var c3 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var c4 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));
			var c5 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c6 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var c7 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var c8 = await input.MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));

			// Group D: Task<Result<T>> source, async fFail
			var d1 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d2 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var d3 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var d4 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => false, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));
			var d5 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d6 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Rnd.Str);
			var d7 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(Rnd.Str));
			var d8 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(failReturn), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(Rnd.Str));

			// Assert
			Assert.Equal(failReturn, a1); Assert.Equal(failReturn, a2); Assert.Equal(failReturn, a3);
			Assert.Equal(failReturn, a4); Assert.Equal(failReturn, a5); Assert.Equal(failReturn, a6); Assert.Equal(failReturn, a7);
			Assert.Equal(failReturn, b1); Assert.Equal(failReturn, b2); Assert.Equal(failReturn, b3);
			Assert.Equal(failReturn, b4); Assert.Equal(failReturn, b5); Assert.Equal(failReturn, b6); Assert.Equal(failReturn, b7);
			Assert.Equal(failReturn, c1); Assert.Equal(failReturn, c2); Assert.Equal(failReturn, c3); Assert.Equal(failReturn, c4);
			Assert.Equal(failReturn, c5); Assert.Equal(failReturn, c6); Assert.Equal(failReturn, c7); Assert.Equal(failReturn, c8);
			Assert.Equal(failReturn, d1); Assert.Equal(failReturn, d2); Assert.Equal(failReturn, d3); Assert.Equal(failReturn, d4);
			Assert.Equal(failReturn, d5); Assert.Equal(failReturn, d6); Assert.Equal(failReturn, d7); Assert.Equal(failReturn, d8);
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

				// Group A: Result<T> source, sync fFail
				var a1 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var a2 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var a3 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));
				var a4 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var a5 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var a6 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var a7 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));

				// Group B: Task<Result<T>> source, sync fFail
				var b1 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var b2 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var b3 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var b4 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));
				var b5 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var b6 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var b7 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));

				// Group C: Result<T> source, async fFail
				var c1 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var c2 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var c3 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var c4 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));
				var c5 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var c6 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var c7 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var c8 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));

				// Group D: Task<Result<T>> source, async fFail
				var d1 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var d2 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var d3 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var d4 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => false, fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));
				var d5 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Rnd.Str);
				var d6 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Rnd.Str);
				var d7 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => falseReturn, fTrue: _ => Task.FromResult(Rnd.Str));
				var d8 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(false), fFalse: _ => Task.FromResult(falseReturn), fTrue: _ => Task.FromResult(Rnd.Str));

				// Assert
				Assert.Equal(falseReturn, a1); Assert.Equal(falseReturn, a2); Assert.Equal(falseReturn, a3);
				Assert.Equal(falseReturn, a4); Assert.Equal(falseReturn, a5); Assert.Equal(falseReturn, a6); Assert.Equal(falseReturn, a7);
				Assert.Equal(falseReturn, b1); Assert.Equal(falseReturn, b2); Assert.Equal(falseReturn, b3);
				Assert.Equal(falseReturn, b4); Assert.Equal(falseReturn, b5); Assert.Equal(falseReturn, b6); Assert.Equal(falseReturn, b7);
				Assert.Equal(falseReturn, c1); Assert.Equal(falseReturn, c2); Assert.Equal(falseReturn, c3); Assert.Equal(falseReturn, c4);
				Assert.Equal(falseReturn, c5); Assert.Equal(falseReturn, c6); Assert.Equal(falseReturn, c7); Assert.Equal(falseReturn, c8);
				Assert.Equal(falseReturn, d1); Assert.Equal(falseReturn, d2); Assert.Equal(falseReturn, d3); Assert.Equal(falseReturn, d4);
				Assert.Equal(falseReturn, d5); Assert.Equal(falseReturn, d6); Assert.Equal(falseReturn, d7); Assert.Equal(falseReturn, d8);
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

				// Group A: Result<T> source, sync fFail
				var a1 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var a2 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var a3 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));
				var a4 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var a5 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var a6 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var a7 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));

				// Group B: Task<Result<T>> source, sync fFail
				var b1 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var b2 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var b3 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var b4 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));
				var b5 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var b6 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var b7 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));

				// Group C: Result<T> source, async fFail
				var c1 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var c2 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var c3 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var c4 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));
				var c5 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var c6 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var c7 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var c8 = await input.MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));

				// Group D: Task<Result<T>> source, async fFail
				var d1 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var d2 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var d3 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var d4 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => true, fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));
				var d5 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => trueReturn);
				var d6 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => trueReturn);
				var d7 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Rnd.Str, fTrue: _ => Task.FromResult(trueReturn));
				var d8 = await input.AsTask().MatchIfAsync(fFail: _ => Task.FromResult(Rnd.Str), fTest: _ => Task.FromResult(true), fFalse: _ => Task.FromResult(Rnd.Str), fTrue: _ => Task.FromResult(trueReturn));

				// Assert
				Assert.Equal(trueReturn, a1); Assert.Equal(trueReturn, a2); Assert.Equal(trueReturn, a3);
				Assert.Equal(trueReturn, a4); Assert.Equal(trueReturn, a5); Assert.Equal(trueReturn, a6); Assert.Equal(trueReturn, a7);
				Assert.Equal(trueReturn, b1); Assert.Equal(trueReturn, b2); Assert.Equal(trueReturn, b3);
				Assert.Equal(trueReturn, b4); Assert.Equal(trueReturn, b5); Assert.Equal(trueReturn, b6); Assert.Equal(trueReturn, b7);
				Assert.Equal(trueReturn, c1); Assert.Equal(trueReturn, c2); Assert.Equal(trueReturn, c3); Assert.Equal(trueReturn, c4);
				Assert.Equal(trueReturn, c5); Assert.Equal(trueReturn, c6); Assert.Equal(trueReturn, c7); Assert.Equal(trueReturn, c8);
				Assert.Equal(trueReturn, d1); Assert.Equal(trueReturn, d2); Assert.Equal(trueReturn, d3); Assert.Equal(trueReturn, d4);
				Assert.Equal(trueReturn, d5); Assert.Equal(trueReturn, d6); Assert.Equal(trueReturn, d7); Assert.Equal(trueReturn, d8);
			}
		}
	}
}
