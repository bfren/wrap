// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class Map_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var f = Substitute.For<Func<string, int>>();

			// Act
			var result = input.Map(f);

			// Assert
			result.AssertFailure(value);
		}

		[Fact]
		public void f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var f = Substitute.For<Func<string, int>>();

			// Act
			_ = input.Map(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Returns_Ok_With_Mapped_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, int>>();
			f.Invoke(Arg.Any<string>()).Returns(value);

			// Act
			var result = input.Map(f);

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public void f_Is_Invoked_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = R.Wrap(value);
			var f = Substitute.For<Func<string, int>>();
			f.Invoke(Arg.Any<string>()).Returns(Rnd.Int);

			// Act
			_ = input.Map(f);

			// Assert
			f.Received(1).Invoke(value);
		}

		[Fact]
		public void f_Throws__Returns_Failure()
		{
			// Arrange
			var input = R.Wrap(Rnd.Str);
			var ex = new Exception(Rnd.Str);

			// Act
			var result = input.Map<string, int>(_ => throw ex);

			// Assert
			result.AssertFailure(ex);
		}

		[Fact]
		public void Custom_Handler_Is_Called_On_Exception()
		{
			// Arrange
			var input = R.Wrap(Rnd.Str);
			var handlerCalled = false;
			R.ExceptionHandler handler = _ => { handlerCalled = true; return FailGen.Create(); };

			// Act
			_ = input.Map<string, int>(_ => throw new Exception(Rnd.Str), handler);

			// Assert
			Assert.True(handlerCalled);
		}

		[Fact]
		public void Custom_Handler_Result_Is_Returned()
		{
			// Arrange
			var customMessage = Rnd.Str;
			var input = R.Wrap(Rnd.Str);
			R.ExceptionHandler handler = _ => FailGen.Create(new(customMessage));

			// Act
			var result = input.Map<string, int>(_ => throw new Exception(Rnd.Str), handler);

			// Assert
			result.AssertFailure(customMessage);
		}
	}
}
