// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class SetValue_Tests
{
	[Fact]
	public void Null_Key__Throws_ArgumentNullException()
	{
		// Arrange
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<string>(mc);

		// Act
		var a0 = void () => cache.SetValue(null!, Rnd.Int64);
		var a1 = void () => cache.SetValue(null!, Rnd.Int64, new());

		// Assert
		Assert.Throws<ArgumentNullException>(a0);
		Assert.Throws<ArgumentNullException>(a1);
	}

	[Fact]
	public void Null_Value__Throws_ArgumentNullException()
	{
		// Arrange
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<long>(mc);

		// Act
		var a0 = void () => cache.SetValue<string>(Rnd.Int64, null!);
		var a1 = void () => cache.SetValue<string>(Rnd.Int64, null!, new());

		// Assert
		Assert.Throws<ArgumentNullException>(a0);
		Assert.Throws<ArgumentNullException>(a1);
	}

	[Fact]
	public void Calls_Cache_CreateEntry__Sets_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Int64;
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<string>(mc);

		// Act
		cache.SetValue(key, value);
		cache.SetValue(key, value, new());

		// Assert
		var entry = mc.Received(2).CreateEntry(key);
		Assert.Equal(value, entry.Value);
	}
}
