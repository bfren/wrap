// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.UnsafeExtensions_Tests;

public class GetEnumerator_Tests
{
	private static IEnumerable<T> Enumerate<TEither, TLeft, T>(Unsafe<TEither, TLeft, T> @unsafe)
		where TEither : IEither<TEither, TLeft, T>
	{
		foreach (var item in @unsafe)
		{
			yield return item;
		}
	}

	public class With_None
	{
		[Fact]
		public void Does_Not_Enumerate()
		{
			// Arrange
			var none = M.Wrap<int?>(null).Unsafe();

			// Act
			var result = Enumerate(none);

			// Assert
			Assert.Empty(result);
		}
	}

	public class With_Some
	{
		[Fact]
		public void Enumerates_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var some = M.Wrap(value).Unsafe();

			// Act
			var result = Enumerate(some);

			// Assert
			Assert.Equal(value, Assert.Single(result));
		}
	}
}
