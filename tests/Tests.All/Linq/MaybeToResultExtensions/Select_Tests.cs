// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Linq.MaybeToResultExtensions_Tests;

// Select is called statically: as an extension method it is ambiguous with MaybeToMaybeExtensions.Select
public class Select_Tests
{
	public class With_Some
	{
		[Fact]
		public async Task Returns_Ok_With_Selected_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = M.Wrap(value);
			static int f(int x) => x + 1;
			static Task<int> fAsync(int x) => Task.FromResult(x + 1);

			// Act
			var r0 = MaybeToResultExtensions.Select(input, f);
			var r1 = await MaybeToResultExtensions.Select(input, fAsync);
			var r2 = await MaybeToResultExtensions.Select(input.AsTask(), f);
			var r3 = await MaybeToResultExtensions.Select(input.AsTask(), fAsync);

			// Assert
			r0.AssertOk(value + 1);
			r1.AssertOk(value + 1);
			r2.AssertOk(value + 1);
			r3.AssertOk(value + 1);
		}

		[Fact]
		public async Task Select_Returns_Null__Returns_Failure()
		{
			// Arrange
			var input = M.Wrap(Rnd.Int);
			static string? f(int _) => null;
			static Task<string?> fAsync(int _) => Task.FromResult<string?>(null);

			// Act
			var r0 = MaybeToResultExtensions.Select(input, f);
			var r1 = await MaybeToResultExtensions.Select(input, fAsync);
			var r2 = await MaybeToResultExtensions.Select(input.AsTask(), f);
			var r3 = await MaybeToResultExtensions.Select(input.AsTask(), fAsync);

			// Assert
			_ = r0.AssertFailure();
			_ = r1.AssertFailure();
			_ = r2.AssertFailure();
			_ = r3.AssertFailure();
		}

		[Fact]
		public async Task Select_Throws__Returns_Failure_With_Exception()
		{
			// Arrange
			var ex = new InvalidOperationException(Rnd.Str);
			var input = M.Wrap(Rnd.Int);
			int f(int _) => throw ex;
			Task<int> fAsync(int _) => throw ex;

			// Act
			var r0 = MaybeToResultExtensions.Select(input, f);
			var r1 = await MaybeToResultExtensions.Select(input, fAsync);
			var r2 = await MaybeToResultExtensions.Select(input.AsTask(), f);
			var r3 = await MaybeToResultExtensions.Select(input.AsTask(), fAsync);

			// Assert
			_ = r0.AssertFailure(ex);
			_ = r1.AssertFailure(ex);
			_ = r2.AssertFailure(ex);
			_ = r3.AssertFailure(ex);
		}

		[Fact]
		public async Task Select_Returns_Faulted_Task__Returns_Failure_With_Exception()
		{
			// Arrange
			var ex = new InvalidOperationException(Rnd.Str);
			var input = M.Wrap(Rnd.Int);
			Task<int> fAsync(int _) => Task.FromException<int>(ex);

			// Act
			var r0 = await MaybeToResultExtensions.Select(input, fAsync);
			var r1 = await MaybeToResultExtensions.Select(input.AsTask(), fAsync);

			// Assert
			_ = r0.AssertFailure(ex);
			_ = r1.AssertFailure(ex);
		}
	}

	public class With_None
	{
		[Fact]
		public async Task Returns_None_Failure_And_Does_Not_Select()
		{
			// Arrange
			var input = default(Maybe<int>);
			var f = Substitute.For<Func<int, int>>();
			var fAsync = Substitute.For<Func<int, Task<int>>>();

			// Act
			var r0 = MaybeToResultExtensions.Select(input, f);
			var r1 = await MaybeToResultExtensions.Select(input, fAsync);
			var r2 = await MaybeToResultExtensions.Select(input.AsTask(), f);
			var r3 = await MaybeToResultExtensions.Select(input.AsTask(), fAsync);

			// Assert
			_ = r0.AssertFailure(C.NoneFailureMessage);
			_ = r1.AssertFailure(C.NoneFailureMessage);
			_ = r2.AssertFailure(C.NoneFailureMessage);
			_ = r3.AssertFailure(C.NoneFailureMessage);
			f.DidNotReceiveWithAnyArgs().Invoke(default);
			_ = fAsync.DidNotReceiveWithAnyArgs().Invoke(default);
		}
	}
}
