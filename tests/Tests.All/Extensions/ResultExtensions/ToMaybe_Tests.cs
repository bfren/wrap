// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class ToMaybe_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Calls_Handler()
		{
			// Arrange
			var input = R.Fail<int>(Rnd.Str);
			var handler = Substitute.For<Action<Failure>>();

			// Act
			_ = input.ToMaybe(handler);
			_ = input.ToMaybe(f => { handler(f); return M.None; });

			// Assert
			handler.ReceivedWithAnyArgs(2).Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public void Returns_None()
		{
			// Arrange
			var input = R.Fail<int>(Rnd.Str);

			// Act
			var result = input.ToMaybe(_ => { });

			// Assert
			result.AssertNone();
		}

		[Fact]
		public void Func_Overload__Returns_Maybe_From_fFail()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Fail<int>(Rnd.Str);

			// Act
			var result = input.ToMaybe(_ => M.Wrap(value));

			// Assert
			result.AssertSome(value);
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Does_Not_Call_Handler()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var handler = Substitute.For<Action<Failure>>();

			// Act
			_ = input.ToMaybe(handler);
			_ = input.ToMaybe(f => { handler(f); return M.None; });

			// Assert
			handler.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public void Returns_Some_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);

			// Act
			var r0 = input.ToMaybe(_ => { });
			var r1 = input.ToMaybe(_ => M.None);

			// Assert
			r0.AssertSome(value);
			r1.AssertSome(value);
		}
	}
}
