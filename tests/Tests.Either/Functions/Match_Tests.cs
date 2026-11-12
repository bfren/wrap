// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Match_Tests
{
	public class With_Return_Value
	{
		[Fact]
		public void Left__Runs_fLeft()
		{
			// Arrange
			var value = Rnd.Str;
			Either<string, int> either = new Left<string>(value);

			// Act
			var result = E.Match<Either<string, int>, string, int, string>(either,
				fLeft: x => $"left {x}",
				fRight: x => $"right {x}"
			);

			// Assert
			Assert.Equal($"left {value}", result);
		}

		[Fact]
		public void Right__Runs_fRight()
		{
			// Arrange
			var value = Rnd.Int;
			Either<string, int> either = new Right<int>(value);

			// Act
			var result = E.Match<Either<string, int>, string, int, string>(either,
				fLeft: x => $"left {x}",
				fRight: x => $"right {x}"
			);

			// Assert
			Assert.Equal($"right {value}", result);
		}
	}

	public class Without_Return_Value
	{
		[Fact]
		public void Left__Runs_fLeft()
		{
			// Arrange
			var value = Rnd.Str;
			var fLeft = Substitute.For<Action<string>>();
			var fRight = Substitute.For<Action<int>>();
			Either<string, int> either = new Left<string>(value);

			// Act
			E.Match(either, fLeft, fRight);

			// Assert
			fLeft.Received(1).Invoke(value);
			fRight.DidNotReceiveWithAnyArgs().Invoke(default);
		}

		[Fact]
		public void Right__Runs_fRight()
		{
			// Arrange
			var value = Rnd.Int;
			var fLeft = Substitute.For<Action<string>>();
			var fRight = Substitute.For<Action<int>>();
			Either<string, int> either = new Right<int>(value);

			// Act
			E.Match(either, fLeft, fRight);

			// Assert
			fLeft.DidNotReceiveWithAnyArgs().Invoke(default!);
			fRight.Received(1).Invoke(value);
		}
	}

	public class Async
	{
		[Fact]
		public async Task Right__Runs_fRight()
		{
			// Arrange
			var value = Rnd.Int;
			Either<string, int> either = new Right<int>(value);

			// Act
			var result = await E.MatchAsync<Either<string, int>, string, int, int>(either.AsTask(),
				fLeft: _ => -1,
				fRight: x => Task.FromResult(x)
			);

			// Assert
			Assert.Equal(value, result);
		}
	}
}
