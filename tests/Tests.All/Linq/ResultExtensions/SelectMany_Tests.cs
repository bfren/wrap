// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Linq.ResultExtensions_Tests;

public class SelectMany_Tests
{
	public class With_Ok
	{
		[Fact]
		public void Inner_Ok__Returns_Ok_With_Selected_Value()
		{
			// Arrange
			var (a, b) = (Rnd.Int, Rnd.Int);

			// Act
			var result = from x in R.Wrap(a)
						 from y in R.Wrap(b)
						 select x + y;

			// Assert
			result.AssertOk(a + b);
		}

		[Fact]
		public async Task Inner_Failure__Returns_Inner_Failure()
		{
			// Arrange
			var message = Rnd.Str;
			var input = R.Wrap(Rnd.Int);
			static int g(int x, int y) => x + y;

			// Act
			var r0 = input.SelectMany(_ => R.Fail<int>(message), g);
			var r1 = await input.SelectMany(_ => R.Fail<int>(message).AsTask(), g);
			var r2 = await input.AsTask().SelectMany(_ => R.Fail<int>(message), g);
			var r3 = await input.AsTask().SelectMany(_ => R.Fail<int>(message).AsTask(), g);

			// Assert
			r0.AssertFailure(message);
			r1.AssertFailure(message);
			r2.AssertFailure(message);
			r3.AssertFailure(message);
		}

		[Fact]
		public async Task Select_Throws__Returns_Failure_With_Exception()
		{
			// Arrange
			var ex = new InvalidOperationException(Rnd.Str);
			var input = R.Wrap(Rnd.Int);
			int g(int x, int y) => throw ex;

			// Act
			var r0 = input.SelectMany(R.Wrap, g);
			var r1 = await input.SelectMany(x => R.Wrap(x).AsTask(), g);
			var r2 = await input.AsTask().SelectMany(R.Wrap, g);
			var r3 = await input.AsTask().SelectMany(x => R.Wrap(x).AsTask(), g);

			// Assert
			r0.AssertFailure(ex);
			r1.AssertFailure(ex);
			r2.AssertFailure(ex);
			r3.AssertFailure(ex);
		}
	}

	public class With_Failure
	{
		[Fact]
		public async Task Returns_Original_Failure_And_Does_Not_Bind()
		{
			// Arrange
			var message = Rnd.Str;
			var input = R.Fail<int>(message);
			var f = Substitute.For<Func<int, Result<int>>>();
			var fAsync = Substitute.For<Func<int, Task<Result<int>>>>();
			static int g(int x, int y) => x + y;

			// Act
			var r0 = input.SelectMany(f, g);
			var r1 = await input.SelectMany(fAsync, g);
			var r2 = await input.AsTask().SelectMany(f, g);
			var r3 = await input.AsTask().SelectMany(fAsync, g);

			// Assert
			r0.AssertFailure(message);
			r1.AssertFailure(message);
			r2.AssertFailure(message);
			r3.AssertFailure(message);
			f.DidNotReceiveWithAnyArgs().Invoke(default);
			await fAsync.DidNotReceiveWithAnyArgs().Invoke(default);
		}
	}
}
