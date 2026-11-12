// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class MatchAsync_Tests
{
	public class Without_Return_Value
	{
		public class Left
		{
			[Fact]
			public async Task Runs_fLeft_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				Either<string, int> either = new Left<string>(value);
				var fLeft = Substitute.For<Action<string>>();
				var fRight = Substitute.For<Action<int>>();

				// Act
				await E.MatchAsync(either, fLeft, async x => fRight(x));
				await E.MatchAsync(either, async x => fLeft(x), fRight);
				await E.MatchAsync(either, async x => fLeft(x), async x => fRight(x));
				await E.MatchAsync(either.AsTask(), async x => fLeft(x), async x => fRight(x));

				// Assert
				fLeft.Received(4).Invoke(value);
				fRight.DidNotReceiveWithAnyArgs().Invoke(default);
			}
		}

		public class Right
		{
			[Fact]
			public async Task Runs_fRight_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				Either<string, int> either = new Right<int>(value);
				var fLeft = Substitute.For<Action<string>>();
				var fRight = Substitute.For<Action<int>>();

				// Act
				await E.MatchAsync(either, fLeft, async x => fRight(x));
				await E.MatchAsync(either, async x => fLeft(x), fRight);
				await E.MatchAsync(either, async x => fLeft(x), async x => fRight(x));
				await E.MatchAsync(either.AsTask(), async x => fLeft(x), async x => fRight(x));

				// Assert
				fLeft.DidNotReceiveWithAnyArgs().Invoke(default!);
				fRight.Received(4).Invoke(value);
			}
		}
	}

	public class With_Return_Value
	{
		public class Left
		{
			[Fact]
			public async Task Returns_fLeft_Value()
			{
				// Arrange
				var value = Rnd.Str;
				Either<string, int> either = new Left<string>(value);
				string fLeft(string x) => $"left {x}";
				string fRight(int x) => $"right {x}";
				var expected = $"left {value}";

				// Act
				var r0 = await E.MatchAsync(either, fLeft, async x => fRight(x));
				var r1 = await E.MatchAsync(either, async x => fLeft(x), fRight);
				var r2 = await E.MatchAsync(either, async x => fLeft(x), async x => fRight(x));
				var r3 = await E.MatchAsync(either.AsTask(), fLeft, fRight);
				var r4 = await E.MatchAsync(either.AsTask(), fLeft, async x => fRight(x));
				var r5 = await E.MatchAsync(either.AsTask(), async x => fLeft(x), fRight);
				var r6 = await E.MatchAsync(either.AsTask(), async x => fLeft(x), async x => fRight(x));

				// Assert
				Assert.All([r0, r1, r2, r3, r4, r5, r6], x => Assert.Equal(expected, x));
			}
		}

		public class Right
		{
			[Fact]
			public async Task Returns_fRight_Value()
			{
				// Arrange
				var value = Rnd.Int;
				Either<string, int> either = new Right<int>(value);
				string fLeft(string x) => $"left {x}";
				string fRight(int x) => $"right {x}";
				var expected = $"right {value}";

				// Act
				var r0 = await E.MatchAsync(either, fLeft, async x => fRight(x));
				var r1 = await E.MatchAsync(either, async x => fLeft(x), fRight);
				var r2 = await E.MatchAsync(either, async x => fLeft(x), async x => fRight(x));
				var r3 = await E.MatchAsync(either.AsTask(), fLeft, fRight);
				var r4 = await E.MatchAsync(either.AsTask(), fLeft, async x => fRight(x));
				var r5 = await E.MatchAsync(either.AsTask(), async x => fLeft(x), fRight);
				var r6 = await E.MatchAsync(either.AsTask(), async x => fLeft(x), async x => fRight(x));

				// Assert
				Assert.All([r0, r1, r2, r3, r4, r5, r6], x => Assert.Equal(expected, x));
			}
		}
	}
}
