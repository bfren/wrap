// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class UnwrapSingle_Tests
{
	private static Func<int> Setup(int defaultValue)
	{
		var fNone = Substitute.For<Func<int>>();
		fNone.Invoke().Returns(defaultValue);
		return fNone;
	}

	public class With_None
	{
		[Fact]
		public void Calls_FNone()
		{
			// Arrange
			var input = NoneGen.Create<List<int>>();
			var fNone = Setup(Rnd.Int);

			// Act
			_ = input.UnwrapSingle(fNone);

			// Assert
			fNone.Received().Invoke();
		}

		[Fact]
		public void Returns_Default_Value()
		{
			// Arrange
			var input = NoneGen.Create<List<int>>();
			var defaultValue = Rnd.Int;
			var fNone = Setup(defaultValue);

			// Act
			var result = input.UnwrapSingle(fNone);

			// Assert
			Assert.Equal(defaultValue, result);
		}
	}

	public class With_Some
	{
		[Fact]
		public void Single_Value__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value });
			var fNone = Setup(Rnd.Int);

			// Act
			var result = input.UnwrapSingle(fNone);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Single_Value__Does_Not_Call_FNone()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value });
			var fNone = Setup(Rnd.Int);

			// Act
			_ = input.UnwrapSingle(fNone);

			// Assert
			fNone.DidNotReceive().Invoke();
		}

		[Fact]
		public void Multiple_Values__Throws_InvalidOperationException()
		{
			// Arrange
			var input = M.Wrap(new List<int> { Rnd.Int, Rnd.Int, Rnd.Int });
			var fNone = Setup(Rnd.Int);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => input.UnwrapSingle(fNone));
		}

		[Fact]
		public void No_Values__Throws_InvalidOperationException()
		{
			// Arrange
			var input = M.Wrap(new List<int>());
			var fNone = Setup(Rnd.Int);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => input.UnwrapSingle(fNone));
		}
	}
}
