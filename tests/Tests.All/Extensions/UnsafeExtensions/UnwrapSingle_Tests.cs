// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class UnwrapSingle_Tests
{
	private static Func<TLeft, int> Setup<TLeft>(int defaultValue)
	{
		var fLeft = Substitute.For<Func<TLeft, int>>();
		fLeft.Invoke(Arg.Any<TLeft>()).Returns(defaultValue);
		return fLeft;
	}

	public class With_Left
	{
		[Fact]
		public void None__Returns_Value_From_FLeft()
		{
			// Arrange
			var input = default(Maybe<List<int>>).Unsafe();
			var defaultValue = Rnd.Int;
			var fLeft = Setup<None>(defaultValue);

			// Act
			var result = input.UnwrapSingle(fLeft);

			// Assert
			Assert.Equal(defaultValue, result);
		}

		[Fact]
		public void Failure__Passes_Failure_To_FLeft()
		{
			// Arrange
			var failure = R.Fail(Rnd.Str);
			var input = R.Fail<List<int>>(failure).Unsafe();
			var fLeft = Setup<Failure>(Rnd.Int);

			// Act
			_ = input.UnwrapSingle(fLeft);

			// Assert
			fLeft.Received(1).Invoke(failure);
		}
	}

	public class With_Right
	{
		[Fact]
		public void Single_Value__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(new List<int> { value }).Unsafe();
			var fLeft = Setup<None>(Rnd.Int);

			// Act
			var result = input.UnwrapSingle(fLeft);

			// Assert
			Assert.Equal(value, result);
			fLeft.DidNotReceive().Invoke(Arg.Any<None>());
		}

		[Fact]
		public void Single_Value_In_Result__Returns_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(new List<int> { value }).Unsafe();
			var fLeft = Setup<Failure>(Rnd.Int);

			// Act
			var result = input.UnwrapSingle(fLeft);

			// Assert
			Assert.Equal(value, result);
		}

		[Fact]
		public void Multiple_Values__Throws_UnsafeUnwrapSingleException()
		{
			// Arrange
			var input = M.Wrap(new List<int> { Rnd.Int, Rnd.Int, Rnd.Int }).Unsafe();
			var fLeft = Setup<None>(Rnd.Int);

			// Act
			var result = Record.Exception(() => input.UnwrapSingle(fLeft));

			// Assert
			_ = Assert.IsType<UnsafeUnwrapSingleException<List<int>, int>>(result);
		}

		[Fact]
		public void No_Values__Throws_UnsafeUnwrapSingleException()
		{
			// Arrange
			var input = M.Wrap(new List<int>()).Unsafe();
			var fLeft = Setup<None>(Rnd.Int);

			// Act
			var result = Record.Exception(() => input.UnwrapSingle(fLeft));

			// Assert
			_ = Assert.IsType<UnsafeUnwrapSingleException<List<int>, int>>(result);
		}

		[Fact]
		public void Endless_List__Reads_Two_Items__Throws_UnsafeUnwrapSingleException()
		{
			// Arrange
			static IEnumerable<int> Endless()
			{
				while (true)
				{
					yield return Rnd.Int;
				}
			}

			var input = M.Wrap(Endless()).Unsafe();
			var fLeft = Setup<None>(Rnd.Int);

			// Act
			var result = Record.Exception(() => input.UnwrapSingle(fLeft));

			// Assert
			_ = Assert.IsType<UnsafeUnwrapSingleException<IEnumerable<int>, int>>(result);
		}
	}
}
