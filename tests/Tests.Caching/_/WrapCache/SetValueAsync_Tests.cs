// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class SetValueAsync_Tests
{
	[Fact]
	public async Task Null_Key__Throws_ArgumentNullException()
	{
		// Arrange
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<string>(mc);

		// Act
		var a0 = Task () => cache.SetValueAsync(null!, async () => Rnd.Lng);
		var a1 = Task () => cache.SetValueAsync(null!, async () => Rnd.Lng, new());

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(a0);
		await Assert.ThrowsAsync<ArgumentNullException>(a1);
	}

	[Fact]
	public async Task Null_Value__Throws_ArgumentNullException()
	{
		// Arrange
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<long>(mc);

		// Act
		var a0 = Task () => cache.SetValueAsync<string>(Rnd.Lng, null!);
		var a1 = Task () => cache.SetValueAsync<string>(Rnd.Lng, null!, new());

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(a0);
		await Assert.ThrowsAsync<ArgumentNullException>(a1);
	}

	[Fact]
	public async Task Calls_Cache_CreateEntry__Sets_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<string>(mc);

		// Act
		await cache.SetValueAsync(key, async () => value);
		await cache.SetValueAsync(key, async () => value, new());

		// Assert
		var entry = mc.Received(2).CreateEntry(key);
		Assert.Equal(value, entry.Value);
	}
}
