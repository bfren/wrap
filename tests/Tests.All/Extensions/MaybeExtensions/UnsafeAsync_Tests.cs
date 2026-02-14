// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class UnsafeAsync_Tests
{
	public class With_None
	{
		[Fact]
		public async Task Returns_Unsafe_With_None()
		{
			// Arrange
			var input = NoneGen.Create<string>();

			// Act
			var result = await input.AsTask().Unsafe();

			// Assert
			Assert.IsType<Unsafe<Maybe<string>, None, string>>(result);
			result.Value.AssertNone();
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Returns_Unsafe_With_Some()
		{
			// Arrange
			var value = Rnd.Str;
			var input = M.Wrap(value);

			// Act
			var result = await input.AsTask().Unsafe();

			// Assert
			Assert.IsType<Unsafe<Maybe<string>, None, string>>(result);
			result.Value.AssertSome(value);
		}
	}
}
