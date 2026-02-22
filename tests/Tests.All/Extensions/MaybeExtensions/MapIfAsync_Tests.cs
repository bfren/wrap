// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class MapIfAsync_Tests
{
	private static (Func<string, bool> fTest, Func<string, Task<int>> fAsync, Func<string, int> fSync) Setup(bool predicateReturn, int mappedValue)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var fAsync = Substitute.For<Func<string, Task<int>>>();
		fAsync.Invoke(Arg.Any<string>()).Returns(Task.FromResult(mappedValue));

		var fSync = Substitute.For<Func<string, int>>();
		fSync.Invoke(Arg.Any<string>()).Returns(mappedValue);

		return (fTest, fAsync, fSync);
	}

	public class With_None
	{
		[Fact]
		public async Task Predicate_Returns_False__Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, fAsync, fSync) = Setup(false, Rnd.Int);

			// Act
			var r0 = await input.MapIfAsync(fTest, fAsync);
			var r1 = await input.AsTask().MapIfAsync(fTest, fSync);
			var r2 = await input.AsTask().MapIfAsync(fTest, fAsync);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
		}

		[Fact]
		public async Task Predicate_Returns_True__Returns_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, fAsync, fSync) = Setup(true, Rnd.Int);

			// Act
			var r0 = await input.MapIfAsync(fTest, fAsync);
			var r1 = await input.AsTask().MapIfAsync(fTest, fSync);
			var r2 = await input.AsTask().MapIfAsync(fTest, fAsync);

			// Assert
			r0.AssertNone();
			r1.AssertNone();
			r2.AssertNone();
		}

		[Fact]
		public async Task Functions_Are_Not_Invoked()
		{
			// Arrange
			var input = NoneGen.Create<string>();
			var (fTest, fAsync, fSync) = Setup(false, Rnd.Int);

			// Act
			_ = await input.MapIfAsync(fTest, fAsync);
			_ = await input.AsTask().MapIfAsync(fTest, fSync);
			_ = await input.AsTask().MapIfAsync(fTest, fAsync);

			// Assert
			fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			await fAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			fSync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_None()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fAsync, fSync) = Setup(false, Rnd.Int);

				// Act
				var r0 = await input.MapIfAsync(fTest, fAsync);
				var r1 = await input.AsTask().MapIfAsync(fTest, fSync);
				var r2 = await input.AsTask().MapIfAsync(fTest, fAsync);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
				r2.AssertNone();
			}

			[Fact]
			public async Task Calls_FTest_Only()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fAsync, fSync) = Setup(false, Rnd.Int);

				// Act
				_ = await input.MapIfAsync(fTest, fAsync);
				_ = await input.AsTask().MapIfAsync(fTest, fSync);
				_ = await input.AsTask().MapIfAsync(fTest, fAsync);

				// Assert
				fTest.Received(3).Invoke(value);
				await fAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				fSync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Mapped_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var mappedValue = Rnd.Int;
				var (fTest, fAsync, fSync) = Setup(true, mappedValue);

				// Act
				var r0 = await input.MapIfAsync(fTest, fAsync);
				var r1 = await input.AsTask().MapIfAsync(fTest, fSync);
				var r2 = await input.AsTask().MapIfAsync(fTest, fAsync);

				// Assert
				r0.AssertSome(mappedValue);
				r1.AssertSome(mappedValue);
				r2.AssertSome(mappedValue);
			}

			[Fact]
			public async Task Calls_FTest_And_F()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var (fTest, fAsync, fSync) = Setup(true, Rnd.Int);

				// Act
				_ = await input.MapIfAsync(fTest, fAsync);
				_ = await input.AsTask().MapIfAsync(fTest, fSync);
				_ = await input.AsTask().MapIfAsync(fTest, fAsync);

				// Assert
				fTest.Received(3).Invoke(value);
				await fAsync.Received(2).Invoke(value);
				fSync.Received().Invoke(value);
			}
		}
	}
}
