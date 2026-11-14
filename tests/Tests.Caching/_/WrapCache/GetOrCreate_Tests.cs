// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class GetOrCreate_Tests
{
	private static IMemoryCache Miss()
	{
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(Arg.Any<string>(), out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = null;
				return false;
			});
		return mc;
	}

	[Fact]
	public void Key_Null__Returns_Failure_With_KeyIsNullMsg()
	{
		// Arrange
		var cache = new WrapCache<string>(Substitute.For<IMemoryCache>());

		// Act
		var r0 = cache.GetOrCreate(null!, () => Rnd.DateTime);
		var r1 = cache.GetOrCreate(null!, () => R.Wrap(Rnd.DateTime));

		// Assert
		r0.AssertFailure("Key cannot be null.");
		r1.AssertFailure("Key cannot be null.");
	}

	[Fact]
	public void Value_Exists__Incorrect_Type__Returns_Failure_With_IncorrectTypeMsg()
	{
		// Arrange
		var key = Rnd.Str;
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = Rnd.Lng;
				return true;
			});
		var cache = new WrapCache<string>(mc);
		const string message = "Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.";

		// Act
		var r0 = cache.GetOrCreate(key, () => Rnd.DateTime);
		var r1 = cache.GetOrCreate(key, () => R.Wrap(Rnd.DateTime));
		var r2 = cache.GetOrCreate(key, () => Rnd.DateTime, new());
		var r3 = cache.GetOrCreate(key, () => R.Wrap(Rnd.DateTime), new());

		// Assert
		r0.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r1.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r2.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r3.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
	}

	[Fact]
	public void Value_Exists__Correct_Type__Returns_Value__Does_Not_Call_Factory()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = value;
				return true;
			});
		var cache = new WrapCache<string>(mc);
		var f0 = Substitute.For<Func<long>>();
		var f1 = Substitute.For<Func<Result<long>>>();

		// Act
		var r0 = cache.GetOrCreate(key, f0);
		var r1 = cache.GetOrCreate(key, f1);

		// Assert
		r0.AssertOk(value);
		r1.AssertOk(value);
		_ = f0.DidNotReceive().Invoke();
		_ = f1.DidNotReceive().Invoke();
	}

	[Fact]
	public void Value_Exists__Is_Null__Calls_Factory__Returns_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = null;
				return true;
			});
		var cache = new WrapCache<string>(mc);

		// Act
		var result = cache.GetOrCreate(key, () => value);

		// Assert
		result.AssertOk(value);
	}

	[Fact]
	public void Value_Does_Not_Exist__Calls_Cache_CreateEntry__Sets_Value__Returns_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var mc = Miss();
		var cache = new WrapCache<string>(mc);

		// Act
		var r0 = cache.GetOrCreate(key, () => value);
		var r1 = cache.GetOrCreate(key, () => R.Wrap(value));
		var r2 = cache.GetOrCreate(key, () => value, new());
		var r3 = cache.GetOrCreate(key, () => R.Wrap(value), new());

		// Assert
		r0.AssertOk(value);
		r1.AssertOk(value);
		r2.AssertOk(value);
		r3.AssertOk(value);
		var entry = mc.Received(4).CreateEntry(key);
		Assert.Equal(value, entry.Value);
	}

	[Fact]
	public void Factory_Returns_Failure__Returns_That_Failure__Does_Not_Store()
	{
		// Arrange
		var key = Rnd.Str;
		var failure = R.Fail(Rnd.Str);
		var mc = Miss();
		var cache = new WrapCache<string>(mc);

		// Act
		var result = cache.GetOrCreate(key, () => R.Fail<long>(failure));

		// Assert
		Assert.Equal(failure, result.AssertFailure());
		_ = mc.DidNotReceive().CreateEntry(Arg.Any<object>());
	}

	[Fact]
	public void Factory_Returns_Null__Returns_Failure__Does_Not_Store()
	{
		// Arrange
		var mc = Miss();
		var cache = new WrapCache<string>(mc);

		// Act
		var result = cache.GetOrCreate(Rnd.Str, () => (string?)null);

		// Assert
		_ = result.AssertFailure();
		_ = mc.DidNotReceive().CreateEntry(Arg.Any<object>());
	}

	[Fact]
	public void Factory_Throws__Returns_Failure_With_ErrorCreatingCacheValueMsg()
	{
		// Arrange
		var key = Rnd.Str;
		var ex = new Exception(Rnd.Str);
		long f0() => throw ex;
		Result<long> f1() => throw ex;
		var cache = new WrapCache<string>(Miss());

		// Act
		var r0 = cache.GetOrCreate(key, f0);
		var r1 = cache.GetOrCreate(key, f1);

		// Assert
		r0.AssertFailure(ex, "Error creating cache value.");
		r1.AssertFailure(ex, "Error creating cache value.");
	}

	[Fact]
	public void Creates_Entry__Waits_For_Expiry__Creates_Again()
	{
		// Arrange
		var key = Rnd.Str;
		var v0 = Rnd.Lng;
		var v1 = Rnd.Lng;
		var cache = new WrapCache<string>(new MemoryCache(new MemoryCacheOptions()));
		var ms = 200;

		// Act
		var r0 = cache.GetOrCreate(key, () => v0, new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(ms) });
		var r1 = cache.GetOrCreate(key, () => v1);
		Thread.Sleep(TimeSpan.FromMilliseconds(ms * 2));
		var r2 = cache.GetOrCreate(key, () => v1);

		// Assert
		r0.AssertOk(v0);
		r1.AssertOk(v0);
		r2.AssertOk(v1);
	}

	[Fact]
	public void Factory_Calls_GetOrCreate_For_Another_Key__Returns_Value()
	{
		// Arrange
		var cache = new WrapCache<int>(new MemoryCache(new MemoryCacheOptions()));
		var inner = Rnd.Int;

		// Act
		var result = cache.GetOrCreate(1, () => cache.GetOrCreate(2, () => inner));

		// Assert
		result.AssertOk(inner);
		cache.GetValue<int>(2).AssertOk(inner);
	}
}
