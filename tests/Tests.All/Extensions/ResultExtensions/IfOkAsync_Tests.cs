// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class IfOkAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<int>(new(value));
			var f = Substitute.For<Action<int>>();

			// Act
			var r0 = await input.IfOkAsync(x => { f(x); return Task.CompletedTask; });
			var r1 = await input.AsTask().IfOkAsync(f);
			var r2 = await input.AsTask().IfOkAsync(x => { f(x); return Task.CompletedTask; });

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
			r2.AssertFailure(value);
		}

		[Fact]
		public async Task Function_Is_Not_Invoked()
		{
			// Arrange
			var input = FailGen.Create<int>();
			var f = Substitute.For<Action<int>>();

			// Act
			_ = await input.IfOkAsync(x => { f(x); return Task.CompletedTask; });
			_ = await input.AsTask().IfOkAsync(f);
			_ = await input.AsTask().IfOkAsync(x => { f(x); return Task.CompletedTask; });

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public async Task Invokes_Function_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<int>>();

			// Act
			_ = await input.IfOkAsync(x => { f(x); return Task.CompletedTask; });
			_ = await input.AsTask().IfOkAsync(f);
			_ = await input.AsTask().IfOkAsync(x => { f(x); return Task.CompletedTask; });

			// Assert
			f.Received(3).Invoke(value);
		}

		[Fact]
		public async Task Returns_Ok()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(value);
			var f = Substitute.For<Action<int>>();

			// Act
			var r0 = await input.IfOkAsync(x => { f(x); return Task.CompletedTask; });
			var r1 = await input.AsTask().IfOkAsync(f);
			var r2 = await input.AsTask().IfOkAsync(x => { f(x); return Task.CompletedTask; });

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
			r2.AssertOk(value);
		}
	}
}
