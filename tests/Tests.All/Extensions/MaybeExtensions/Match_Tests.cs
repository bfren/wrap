// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class Match_Tests
{
	public class Without_Return_Value
	{
		public class With_None
		{
			[Fact]
			public void Calls_FNone()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<int>>();

				// Act
				input.Match(fNone, fSome);

				// Assert
				fNone.Received(1).Invoke();
				fSome.DidNotReceive().Invoke(Arg.Any<int>());
			}
		}

		public class With_Some
		{
			[Fact]
			public void Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<int>>();

				// Act
				input.Match(fNone, fSome);

				// Assert
				fSome.Received(1).Invoke(value);
				fNone.DidNotReceive().Invoke();
			}
		}
	}

	public class With_Return_Value
	{
		public class With_None
		{
			[Fact]
			public void Calls_FNone()
			{
				// Arrange
				Maybe<int> input = M.None;
				var fNone = Substitute.For<Func<string>>();
				var fSome = Substitute.For<Func<int, string>>();

				// Act
				_ = input.Match(fNone, fSome);

				// Assert
				fNone.Received(1).Invoke();
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}

			[Fact]
			public void Returns_FNone_Value()
			{
				// Arrange
				Maybe<int> input = M.None;
				var value = Rnd.Str;
				var fNone = Substitute.For<Func<string>>();
				fNone.Invoke().Returns(value);

				// Act
				var result = input.Match(fNone, Substitute.For<Func<int, string>>());

				// Assert
				Assert.Equal(value, result);
			}
		}

		public class With_Some
		{
			[Fact]
			public void Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<string>>();
				var fSome = Substitute.For<Func<int, string>>();

				// Act
				_ = input.Match(fNone, fSome);

				// Assert
				fSome.Received(1).Invoke(value);
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public void Returns_FSome_Value()
			{
				// Arrange
				var input = M.Wrap(Rnd.Int);
				var value = Rnd.Str;
				var fSome = Substitute.For<Func<int, string>>();
				fSome.Invoke(default).ReturnsForAnyArgs(value);

				// Act
				var result = input.Match(Substitute.For<Func<string>>(), fSome);

				// Assert
				Assert.Equal(value, result);
			}
		}
	}
}
