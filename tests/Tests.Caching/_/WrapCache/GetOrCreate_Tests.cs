// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class GetOrCreate_Tests
{
	[Fact]
	public void Key_Null__Returns_None_With_KeyIsNullMsg()
	{
		// Arrange
		var mc = Substitute.For<IMemoryCache>();
		var c0 = new WrapCache<string>(mc);
		var c1 = new WrapCache<string>(mc);

		// Act
		var r0 = c0.GetOrCreate(null!, () => Rnd.DateTime);
		var r1 = c1.GetOrCreate(null!, () => M.Wrap(Rnd.DateTime));

		// Assert
		r0.AssertNone();
		c0.AssertFailure("Key cannot be null.");
		r1.AssertNone();
		c1.AssertFailure("Key cannot be null.");
	}

	[Fact]
	public void Value_Exists__Incorrect_Type__Returns_None_With_CacheEntryIsIncorrectTypeMsg()
	{
		// Arrange
		var key = Rnd.Str;
		var mc = Substitute.For<IMemoryCache>();
		var c0 = new WrapCache<string>(mc);
		var c1 = new WrapCache<string>(mc);
		var c2 = new WrapCache<string>(mc);
		var c3 = new WrapCache<string>(mc);
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = Rnd.Lng;
				return true;
			});

		// Act
		var r0 = c0.GetOrCreate(key, () => Rnd.DateTime);
		var r1 = c1.GetOrCreate(key, () => M.Wrap(Rnd.DateTime));
		var r2 = c2.GetOrCreate(key, () => Rnd.DateTime, new());
		var r3 = c3.GetOrCreate(key, () => M.Wrap(Rnd.DateTime), new());

		// Assert
		r0.AssertNone();
		c0.AssertFailure(
			"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
			key, nameof(Int64), nameof(DateTime)
		);
		r1.AssertNone();
		c1.AssertFailure(
			"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
			key, nameof(Int64), nameof(DateTime)
		);
		r2.AssertNone();
		c2.AssertFailure(
			"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
			key, nameof(Int64), nameof(DateTime)
		);
		r3.AssertNone();
		c3.AssertFailure(
			"Requested cache entry '{Key}' is of type '{IncorrectType}' not the requested type '{RequestedType}'.",
			key, nameof(Int64), nameof(DateTime)
		);
	}

	[Fact]
	public void Value_Exists__Is_Null__Returns_None()
	{
		// Arrange
		var key = Rnd.Str;
		var mc = Substitute.For<IMemoryCache>();
		var cache = new WrapCache<string>(mc);
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = null;
				return true;
			});

		// Act
		var r0 = cache.GetOrCreate(key, () => Rnd.DateTime);
		var r1 = cache.GetOrCreate(key, () => M.Wrap(Rnd.DateTime));
		var r2 = cache.GetOrCreate(key, () => Rnd.DateTime, new());
		var r3 = cache.GetOrCreate(key, () => M.Wrap(Rnd.DateTime), new());

		// Assert
		r0.AssertNone();
		r1.AssertNone();
		r2.AssertNone();
		r3.AssertNone();
	}

	[Fact]
	public void Value_Exists__Correct_Type__Returns_Value()
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

		// Act
		var r0 = cache.GetOrCreate(key, () => Rnd.Lng);
		var r1 = cache.GetOrCreate(key, () => M.Wrap(Rnd.Lng));
		var r2 = cache.GetOrCreate(key, () => Rnd.Lng, new());
		var r3 = cache.GetOrCreate(key, () => M.Wrap(Rnd.Lng), new());

		// Assert
		r0.AssertSome(value);
		r1.AssertSome(value);
		r2.AssertSome(value);
		r3.AssertSome(value);
	}

	[Fact]
	public void Value_Does_Not_Exist__Calls_Cache_CreateEntry__Sets_Value__Returns_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var f0 = Substitute.For<Func<long>>();
		f0.Invoke()
			.Returns(value);
		var f1 = Substitute.For<Func<Maybe<long>>>();
		f1.Invoke()
			.Returns(value);
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(Arg.Any<string>(), out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = null;
				return false;
			});
		var cache = new WrapCache<string>(mc);

		// Act
		var r0 = cache.GetOrCreate(key, f0);
		var r1 = cache.GetOrCreate(key, f1);
		var r2 = cache.GetOrCreate(key, f0, new());
		var r3 = cache.GetOrCreate(key, f1, new());

		// Assert
		r0.AssertSome(value);
		r1.AssertSome(value);
		r2.AssertSome(value);
		r3.AssertSome(value);
		var entry = mc.Received(4).CreateEntry(key);
		Assert.Equal(value, entry.Value);
	}

	[Fact]
	public void Caches_Exception_In_ValueFactory__Returns_None_With_ErrorCreatingCacheValueMsg()
	{
		// Arrange
		var key = Rnd.Str;
		var ex = new Exception(Rnd.Str);
		long f0() => throw ex;
		Maybe<long> f1() => throw ex;
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(Arg.Any<string>(), out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = null;
				return false;
			});
		var c0 = new WrapCache<string>(mc);
		var c1 = new WrapCache<string>(mc);
		var c2 = new WrapCache<string>(mc);
		var c3 = new WrapCache<string>(mc);

		// Act
		var r0 = c0.GetOrCreate(key, f0);
		var r1 = c1.GetOrCreate(key, f1);
		var r2 = c2.GetOrCreate(key, f0, new());
		var r3 = c3.GetOrCreate(key, f1, new());

		// Assert
		r0.AssertNone();
		c0.AssertFailure(ex, "Error creating cache value.");
		r1.AssertNone();
		c1.AssertFailure(ex, "Error creating cache value.");
		r2.AssertNone();
		c2.AssertFailure(ex, "Error creating cache value.");
		r3.AssertNone();
		c3.AssertFailure(ex, "Error creating cache value.");
	}

	[Fact]
	public void Multiple_Threads__Calls_CreateEntry__Once()
	{
		// Arrange
		var key = Rnd.Str;
		var created = false;
		var f = Substitute.For<Func<Task<long>>>();
		var mc = Substitute.For<IMemoryCache>();
		mc.TryGetValue(key, out Arg.Any<object>()!)
			.Returns(x =>
			{
				x[1] = Rnd.Lng;
				return created;
			});
		mc.When(x => x.CreateEntry(key))
			.Do(Callback.First(_ => created = true));
		var cache = new WrapCache<string>(mc);

		// Act
		Parallel.ForEach(
			source: Enumerable.Range(0, 30),
			parallelOptions: new() { MaxDegreeOfParallelism = 10 },
			body: _ => cache.GetOrCreate(key, f)
		);

		// Assert
		mc.Received(1).CreateEntry(key);
	}

	[Fact]
	public void Creates_Entry__Waits_For_Expiry__Creates_Again()
	{
		// Arrange
		var key = Rnd.Str;
		var v0 = Rnd.Lng;
		var v1 = Rnd.Lng;
		var mc = new MemoryCache(new MemoryCacheOptions());
		var ms = 200;
		var cache = new WrapCache<string>(mc);

		// Act
		var r0 = cache.GetOrCreate(key, () => v0, new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(ms) });
		var r1 = cache.GetOrCreate(key, () => v1);
		Thread.Sleep(TimeSpan.FromMilliseconds(ms * 2));
		var r2 = cache.GetOrCreate(key, () => v1);

		// Assert
		r0.AssertSome(v0);
		r1.AssertSome(v0);
		r2.AssertSome(v1);
	}
}
