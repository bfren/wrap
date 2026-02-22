// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MonadExtensions_Tests;

public class ToResult_Tests
{
	public class Value
	{
		[Fact]
		public void Returns_Ok_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var monad = Monad<int>.Wrap(value);

			// Act
			var result = monad.ToResult();

			// Assert
			result.AssertOk(value);
		}

		[Fact]
		public async Task Async__Returns_Ok_With_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var monad = Monad<int>.Wrap(value);

			// Act
			var r0 = await monad.ToResultAsync();
			var r1 = await Task.FromResult<IMonad<int>>(monad).ToResultAsync();

			// Assert
			r0.AssertOk(value);
			r1.AssertOk(value);
		}
	}

	public class Enumerable
	{
		[Fact]
		public void Returns_Values_For_Each_Item()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var monads = new List<IMonad<int>> { Monad<int>.Wrap(v0), Monad<int>.Wrap(v1) };

			// Act
			var results = monads.ToResult().ToList();

			// Assert
			Assert.Equal(2, results.Count);
			Assert.Collection(results,
				x => x.AssertOk(v0),
				x => x.AssertOk(v1)
			);
		}
	}

	[Fact]
	public void Skips_Null_Items()
	{
		// Arrange
		var value = Rnd.Int;
		IMonad<int> nullItem = null!;
		var monads = new List<IMonad<int>> { Monad<int>.Wrap(value), nullItem };

		// Act
		var results = monads.ToResult().ToList();

		// Assert
		Assert.Single(results).AssertOk(value);
	}

	[Fact]
	public async Task Async__Returns_Values_For_Each_Item()
	{
		// Arrange
		var v0 = Rnd.Int;
		var v1 = Rnd.Int;
		var monads = new List<IMonad<int>> { Monad<int>.Wrap(v0), Monad<int>.Wrap(v1) };

		// Act
		var r0 = (await monads.ToResultAsync()).ToList();
		var r1 = (await Task.FromResult<IEnumerable<IMonad<int>>>(monads).ToResultAsync()).ToList();

		// Assert
		Assert.Equal(2, r0.Count);
		Assert.Collection(r0,
			x => x.AssertOk(v0),
			x => x.AssertOk(v1)
		);
		Assert.Equal(2, r1.Count);
		Assert.Collection(r1,
			x => x.AssertOk(v0),
			x => x.AssertOk(v1)
		);
	}
}
