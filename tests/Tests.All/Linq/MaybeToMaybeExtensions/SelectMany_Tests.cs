// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Linq.MaybeToMaybeExtensions_Tests;

public class SelectMany_Tests
{
	public class With_Some
	{
		[Fact]
		public async Task Inner_Some__Returns_Some_With_Selected_Value()
		{
			// Arrange
			var (a, b) = (Rnd.Int, Rnd.Int);
			var input = M.Wrap(a);
			static int g(int x, int y) => x + y;

			// Act
			var r0 = input.SelectMany(_ => M.Wrap(b), g);
			var r1 = await input.SelectMany(_ => M.Wrap(b).AsTask(), g);
			var r2 = await input.AsTask().SelectMany(_ => M.Wrap(b), g);
			var r3 = await input.AsTask().SelectMany(_ => M.Wrap(b).AsTask(), g);

			// Assert
			r0.AssertSome(a + b);
			r1.AssertSome(a + b);
			r2.AssertSome(a + b);
			r3.AssertSome(a + b);
		}

		[Fact]
		public async Task Inner_None__Returns_None()
		{
			// Arrange
			var input = M.Wrap(Rnd.Int);
			static int g(int x, int y) => x + y;

			// Act
			var r0 = input.SelectMany(_ => default(Maybe<int>), g);
			var r1 = await input.SelectMany(_ => default(Maybe<int>).AsTask(), g);
			var r2 = await input.AsTask().SelectMany(_ => default(Maybe<int>), g);
			var r3 = await input.AsTask().SelectMany(_ => default(Maybe<int>).AsTask(), g);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
			r3.AssertNone();
		}
	}

	public class With_None
	{
		[Fact]
		public async Task Returns_None_And_Does_Not_Bind()
		{
			// Arrange
			var input = default(Maybe<int>);
			var f = Substitute.For<Func<int, Maybe<int>>>();
			var fAsync = Substitute.For<Func<int, Task<Maybe<int>>>>();
			static int g(int x, int y) => x + y;

			// Act
			var r0 = input.SelectMany(f, g);
			var r1 = await input.SelectMany(fAsync, g);
			var r2 = await input.AsTask().SelectMany(f, g);
			var r3 = await input.AsTask().SelectMany(fAsync, g);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
			r3.AssertNone();
			f.DidNotReceiveWithAnyArgs().Invoke(default);
			await fAsync.DidNotReceiveWithAnyArgs().Invoke(default);
		}
	}
}
