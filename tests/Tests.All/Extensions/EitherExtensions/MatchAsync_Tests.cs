// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EitherExtensions_Tests;

public class MatchAsync_Tests
{
	public class Without_Return_Value
	{
		public class With_Left
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
				await either.MatchAsync(fLeft, async x => fRight(x));
				await either.MatchAsync(async x => fLeft(x), fRight);
				await either.MatchAsync(async x => fLeft(x), async x => fRight(x));
				await either.AsTask().MatchAsync(fLeft, fRight);
				await either.AsTask().MatchAsync(fLeft, async x => fRight(x));
				await either.AsTask().MatchAsync(async x => fLeft(x), fRight);
				await either.AsTask().MatchAsync(async x => fLeft(x), async x => fRight(x));

				// Assert
				fLeft.Received(7).Invoke(value);
				fRight.DidNotReceiveWithAnyArgs().Invoke(default);
			}
		}

		public class With_Right
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
				await either.MatchAsync(fLeft, async x => fRight(x));
				await either.MatchAsync(async x => fLeft(x), fRight);
				await either.MatchAsync(async x => fLeft(x), async x => fRight(x));
				await either.AsTask().MatchAsync(fLeft, fRight);
				await either.AsTask().MatchAsync(fLeft, async x => fRight(x));
				await either.AsTask().MatchAsync(async x => fLeft(x), fRight);
				await either.AsTask().MatchAsync(async x => fLeft(x), async x => fRight(x));

				// Assert
				fLeft.DidNotReceiveWithAnyArgs().Invoke(default!);
				fRight.Received(7).Invoke(value);
			}
		}
	}
}
