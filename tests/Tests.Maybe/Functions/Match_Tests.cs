// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Match_Tests
{
	public class Without_Return_Value
	{
		public class Null_Maybe : Abstracts.Match_Tests.Null_Maybe
		{
			[Fact]
			public override void Test00_Throws_NullMaybeException() =>
				Test00<int>(m => M.Match(m, Substitute.For<Action>(), Substitute.For<Action<int>>()));
		}

		public class Invalid_Maybe : Abstracts.Match_Tests.Unknown_Maybe
		{
			[Fact]
			public override void Test01_Throws_InvalidMaybeTypeException() =>
				Test00<int>(m => M.Match(m, Substitute.For<Action>(), Substitute.For<Action<int>>()));
		}

		public class None
		{
			[Fact]
			public void Executes_None_Action()
			{
				// Arrange
				Maybe<int> maybe = M.None;
				var none = Substitute.For<Action>();
				var some = Substitute.For<Action<int>>();

				// Act
				M.Match(maybe, none, some);

				// Assert
				none.Received(1).Invoke();
				some.DidNotReceive().Invoke(Arg.Any<int>());
			}
		}

		public class Some
		{
			[Fact]
			public void Executes_Some_Action_With_Correct_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var maybe = M.Wrap(value);
				var none = Substitute.For<Action>();
				var some = Substitute.For<Action<int>>();

				// Act
				M.Match(maybe, none, some);

				// Assert
				some.Received(1).Invoke(value);
				none.DidNotReceive().Invoke();
			}
		}
	}

	public class With_Return_Value
	{
		public class Null_Maybe : Abstracts.Match_Tests.Null_Maybe
		{
			[Fact]
			public override void Test00_Throws_NullMaybeException() =>
				Test00<int>(m => M.Match(m, Substitute.For<Func<int>>(), Substitute.For<Func<int, int>>()));
		}
		public class Invalid_Maybe : Abstracts.Match_Tests.Unknown_Maybe
		{
			[Fact]
			public override void Test01_Throws_InvalidMaybeTypeException() =>
				Test00<int>(m => M.Match(m, Substitute.For<Func<int>>(), Substitute.For<Func<int, int>>()));
		}
	}
}
