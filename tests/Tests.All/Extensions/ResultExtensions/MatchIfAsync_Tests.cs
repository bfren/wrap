// Wrap: Unit Tests
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
			var value = Rnd.Str;
			var input = FailGen.Create<int>();

			// Act

			// Group A: Result<T> source, sync fFail
			var a1 = await input.MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var a2 = await input.MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var a3 = await input.MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var a4 = await input.MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var a5 = await input.MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var a6 = await input.MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var a7 = await input.MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);

			// Group B: Task<Result<T>> source, sync fFail
			var b1 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b2 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b3 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var b4 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var b5 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b6 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var b7 = await input.AsTask().MatchIfAsync(fFail: _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);

			// Group C: Result<T> source, async fFail
			var c1 = await input.MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c2 = await input.MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c3 = await input.MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var c4 = await input.MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var c5 = await input.MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c6 = await input.MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var c7 = await input.MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var c8 = await input.MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);

			// Group D: Task<Result<T>> source, async fFail
			var d1 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d2 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d3 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var d4 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var d5 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d6 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: _ => Rnd.Str);
			var d7 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: _ => Rnd.Str, fTrue: async _ => Rnd.Str);
			var d8 = await input.AsTask().MatchIfAsync(fFail: async _ => value, fTest: async _ => false, fFalse: async _ => Rnd.Str, fTrue: async _ => Rnd.Str);

			// Assert
			Assert.Equal(value, a1);
			Assert.Equal(value, a2);
			Assert.Equal(value, a3);
			Assert.Equal(value, a4);
			Assert.Equal(value, a5);
			Assert.Equal(value, a6);
			Assert.Equal(value, a7);
			Assert.Equal(value, b1);
			Assert.Equal(value, b2);
			Assert.Equal(value, b3);
			Assert.Equal(value, b4);
			Assert.Equal(value, b5);
			Assert.Equal(value, b6);
			Assert.Equal(value, b7);
			Assert.Equal(value, c1);
			Assert.Equal(value, c2);
			Assert.Equal(value, c3);
			Assert.Equal(value, c4);
			Assert.Equal(value, c5);
			Assert.Equal(value, c6);
			Assert.Equal(value, c7);
			Assert.Equal(value, c8);
			Assert.Equal(value, d1);
			Assert.Equal(value, d2);
			Assert.Equal(value, d3);
			Assert.Equal(value, d4);
			Assert.Equal(value, d5);
			Assert.Equal(value, d6);
			Assert.Equal(value, d7);
			Assert.Equal(value, d8);
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
				var value = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act

				// Group A: Result<T> source, sync fFail
				var a1 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var a2 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var a3 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);
				var a4 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var a5 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var a6 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var a7 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);

				// Group B: Task<Result<T>> source, sync fFail
				var b1 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var b2 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var b3 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var b4 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);
				var b5 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var b6 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var b7 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);

				// Group C: Result<T> source, async fFail
				var c1 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var c2 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var c3 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var c4 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);
				var c5 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var c6 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var c7 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var c8 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);

				// Group D: Task<Result<T>> source, async fFail
				var d1 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var d2 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var d3 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var d4 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);
				var d5 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: _ => Rnd.Str);
				var d6 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: _ => Rnd.Str);
				var d7 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: _ => value, fTrue: async _ => Rnd.Str);
				var d8 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => false, fFalse: async _ => value, fTrue: async _ => Rnd.Str);

				// Assert
				Assert.Equal(value, a1);
				Assert.Equal(value, a2);
				Assert.Equal(value, a3);
				Assert.Equal(value, a4);
				Assert.Equal(value, a5);
				Assert.Equal(value, a6);
				Assert.Equal(value, a7);
				Assert.Equal(value, b1);
				Assert.Equal(value, b2);
				Assert.Equal(value, b3);
				Assert.Equal(value, b4);
				Assert.Equal(value, b5);
				Assert.Equal(value, b6);
				Assert.Equal(value, b7);
				Assert.Equal(value, c1);
				Assert.Equal(value, c2);
				Assert.Equal(value, c3);
				Assert.Equal(value, c4);
				Assert.Equal(value, c5);
				Assert.Equal(value, c6);
				Assert.Equal(value, c7);
				Assert.Equal(value, c8);
				Assert.Equal(value, d1);
				Assert.Equal(value, d2);
				Assert.Equal(value, d3);
				Assert.Equal(value, d4);
				Assert.Equal(value, d5);
				Assert.Equal(value, d6);
				Assert.Equal(value, d7);
				Assert.Equal(value, d8);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_fTrue_Result()
			{
				// Arrange
				var value = Rnd.Str;
				var input = R.Wrap(Rnd.Int);

				// Act

				// Group A: Result<T> source, sync fFail
				var a1 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var a2 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var a3 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);
				var a4 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var a5 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var a6 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var a7 = await input.MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);

				// Group B: Task<Result<T>> source, sync fFail
				var b1 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var b2 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var b3 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var b4 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);
				var b5 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var b6 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var b7 = await input.AsTask().MatchIfAsync(fFail: _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);

				// Group C: Result<T> source, async fFail
				var c1 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var c2 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var c3 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var c4 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);
				var c5 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var c6 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var c7 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var c8 = await input.MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);

				// Group D: Task<Result<T>> source, async fFail
				var d1 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var d2 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var d3 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var d4 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);
				var d5 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: _ => value);
				var d6 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: _ => value);
				var d7 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: _ => Rnd.Str, fTrue: async _ => value);
				var d8 = await input.AsTask().MatchIfAsync(fFail: async _ => Rnd.Str, fTest: async _ => true, fFalse: async _ => Rnd.Str, fTrue: async _ => value);

				// Assert
				Assert.Equal(value, a1);
				Assert.Equal(value, a2);
				Assert.Equal(value, a3);
				Assert.Equal(value, a4);
				Assert.Equal(value, a5);
				Assert.Equal(value, a6);
				Assert.Equal(value, a7);
				Assert.Equal(value, b1);
				Assert.Equal(value, b2);
				Assert.Equal(value, b3);
				Assert.Equal(value, b4);
				Assert.Equal(value, b5);
				Assert.Equal(value, b6);
				Assert.Equal(value, b7);
				Assert.Equal(value, c1);
				Assert.Equal(value, c2);
				Assert.Equal(value, c3);
				Assert.Equal(value, c4);
				Assert.Equal(value, c5);
				Assert.Equal(value, c6);
				Assert.Equal(value, c7);
				Assert.Equal(value, c8);
				Assert.Equal(value, d1);
				Assert.Equal(value, d2);
				Assert.Equal(value, d3);
				Assert.Equal(value, d4);
				Assert.Equal(value, d5);
				Assert.Equal(value, d6);
				Assert.Equal(value, d7);
				Assert.Equal(value, d8);
			}
		}
	}
}
