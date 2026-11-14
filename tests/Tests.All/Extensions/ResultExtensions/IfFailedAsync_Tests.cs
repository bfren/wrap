// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfFailedAsync_Tests
{
	public class With_Ok
	{
		[Fact]
		public async Task Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<Failure>>();

			// Act
			var r0 = await input.IfFailedAsync(async x => f(x));
			var r1 = await input.AsTask().IfFailedAsync(f);
			var r2 = await input.AsTask().IfFailedAsync(async x => f(x));

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
			r2.AssertOk(value);
		}

		[Fact]
		public async Task Function_Is_Not_Invoked()
		{
			// Arrange
			var input = R.Wrap(Rnd.Int);
			var f = Substitute.For<Action<Failure>>();

			// Act
			_ = await input.IfFailedAsync(async x => f(x));
			_ = await input.AsTask().IfFailedAsync(f);
			_ = await input.AsTask().IfFailedAsync(async x => f(x));

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Failure>());
		}
	}

	public class With_Failure
	{
		[Fact]
		public async Task Invokes_Function_With_Failure_Value()
		{
			// Arrange
			var input = R.Fail<int>(Rnd.Str);
			var f = Substitute.For<Action<Failure>>();

			// Act
			_ = await input.IfFailedAsync(async x => f(x));
			_ = await input.AsTask().IfFailedAsync(f);
			_ = await input.AsTask().IfFailedAsync(async x => f(x));

			// Assert
			f.Received(3).Invoke(Arg.Any<Failure>());
		}

		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = R.Fail<int>(value);
			var f = Substitute.For<Action<Failure>>();

			// Act
			var r0 = await input.IfFailedAsync(async x => f(x));
			var r1 = await input.AsTask().IfFailedAsync(f);
			var r2 = await input.AsTask().IfFailedAsync(async x => f(x));

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}
	}
}
