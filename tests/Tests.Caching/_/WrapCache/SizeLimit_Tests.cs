// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class SizeLimit_Tests
{
	private static WrapCache<int> CreateCache() =>
		new(new MemoryCache(new MemoryCacheOptions { SizeLimit = 100 }));

	public class Default_Options
	{
		[Fact]
		public async Task SetValue__Stores_Value()
		{
			// Arrange
			var cache = CreateCache();
			var v0 = Rnd.Str;
			var v1 = Rnd.Str;

			// Act
			cache.SetValue(0, v0);
			await cache.SetValueAsync(1, () => Task.FromResult(v1));

			// Assert
			cache.GetValue<string>(0).AssertOk(v0);
			cache.GetValue<string>(1).AssertOk(v1);
		}

		[Fact]
		public void GetOrCreate__Stores_Value()
		{
			// Arrange
			var cache = CreateCache();
			var v0 = Rnd.Str;
			var v1 = Rnd.Str;

			// Act
			var r0 = cache.GetOrCreate(0, () => v0);
			var r1 = cache.GetOrCreate(1, () => R.Wrap(v1));

			// Assert
			r0.AssertOk(v0);
			r1.AssertOk(v1);
			cache.GetValue<string>(0).AssertOk(v0);
			cache.GetValue<string>(1).AssertOk(v1);
		}

		[Fact]
		public async Task GetOrCreateAsync__Stores_Value()
		{
			// Arrange
			var cache = CreateCache();
			var v0 = Rnd.Str;
			var v1 = Rnd.Str;

			// Act
			var r0 = await cache.GetOrCreateAsync(0, () => Task.FromResult(v0));
			var r1 = await cache.GetOrCreateAsync(1, () => Task.FromResult(R.Wrap(v1)));

			// Assert
			r0.AssertOk(v0);
			r1.AssertOk(v1);
			cache.GetValue<string>(0).AssertOk(v0);
			cache.GetValue<string>(1).AssertOk(v1);
		}
	}

	public class Options_Without_Size
	{
		[Fact]
		public void SetValue__Throws_InvalidOperationException()
		{
			// Arrange
			var cache = CreateCache();

			// Act
			var action = void () => cache.SetValue(0, Rnd.Str, new());

			// Assert
			_ = Assert.Throws<InvalidOperationException>(action);
		}

		[Fact]
		public void GetOrCreate__Returns_Failure__Does_Not_Store()
		{
			// Arrange
			var cache = CreateCache();

			// Act
			var result = cache.GetOrCreate(0, () => Rnd.Str, new());

			// Assert
			var f = result.AssertFailure("Error creating cache value.");
			Assert.IsType<InvalidOperationException>(f.Exception);
			_ = cache.GetValue<string>(0).AssertFailure("Cache entry '{Key}' not found.", 0);
		}
	}
}
