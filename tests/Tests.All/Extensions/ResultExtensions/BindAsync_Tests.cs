// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class BindAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<string>(new(value));
			var f = Substitute.For<Func<string, Result<int>>>();

			// Act
			var r0 = await input.BindAsync(async x => f(x));
			var r1 = await input.AsTask().BindAsync(f);
			var r2 = await input.AsTask().BindAsync(async x => f(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}

		[Fact]
		public async Task f_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<string>();
			var f = Substitute.For<Func<string, Result<int>>>();

			// Act
			_ = await input.BindAsync(async x => f(x));
			_ = await input.AsTask().BindAsync(f);
			_ = await input.AsTask().BindAsync(async x => f(x));

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Returns_Result_Of_f()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(value);

			// Act
			var r0 = await input.BindAsync(async x => f(x));
			var r1 = await input.AsTask().BindAsync(f);
			var r2 = await input.AsTask().BindAsync(async x => f(x));

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
			r2.AssertOk(value);
		}

		[Fact]
		public async Task f_Is_Invoked_With_Value()
		{
			// Arrange
			var value = Rnd.Str;
			var input = R.Wrap(value);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(Rnd.Int);

			// Act
			_ = await input.BindAsync(async x => f(x));
			_ = await input.AsTask().BindAsync(f);
			_ = await input.AsTask().BindAsync(async x => f(x));

			// Assert
			f.Received(3).Invoke(value);
		}

		[Fact]
		public async Task f_Returns_Failure__Returns_Failure()
		{
			// Arrange
			var failValue = Rnd.Str;
			var input = R.Wrap(Rnd.Str);
			var f = Substitute.For<Func<string, Result<int>>>();
			f.Invoke(Arg.Any<string>()).Returns(FailGen.Create<int>(new(failValue)));

			// Act
			var r0 = await input.BindAsync(async x => f(x));
			var r1 = await input.AsTask().BindAsync(f);
			var r2 = await input.AsTask().BindAsync(async x => f(x));

			// Assert
			r0.AssertFailure(failValue);
			r1.AssertFailure(failValue);
			r2.AssertFailure(failValue);
		}
	}
}
