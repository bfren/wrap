// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.Caching.Memory;

namespace Wrap.Caching.WrapCache_Tests;

public class GetOrCreateAsync_Tests
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
	public async Task Key_Null__Returns_Failure_With_KeyIsNullMsg()
	{
		// Arrange
		var cache = new WrapCache<string>(Substitute.For<IMemoryCache>());

		// Act
		var r0 = await cache.GetOrCreateAsync(null!, () => Task.FromResult(Rnd.DateTime));
		var r1 = await cache.GetOrCreateAsync(null!, () => Task.FromResult(R.Wrap(Rnd.DateTime)));

		// Assert
		r0.AssertFailure("Key cannot be null.");
		r1.AssertFailure("Key cannot be null.");
	}

	[Fact]
	public async Task Value_Exists__Incorrect_Type__Returns_Failure_With_IncorrectTypeMsg()
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
		var r0 = await cache.GetOrCreateAsync(key, () => Task.FromResult(Rnd.DateTime));
		var r1 = await cache.GetOrCreateAsync(key, () => Task.FromResult(R.Wrap(Rnd.DateTime)));
		var r2 = await cache.GetOrCreateAsync(key, () => Task.FromResult(Rnd.DateTime), new());
		var r3 = await cache.GetOrCreateAsync(key, () => Task.FromResult(R.Wrap(Rnd.DateTime)), new());

		// Assert
		r0.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r1.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r2.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
		r3.AssertFailure(message, key, nameof(Int64), nameof(DateTime));
	}

	[Fact]
	public async Task Value_Exists__Correct_Type__Returns_Value__Does_Not_Call_Factory()
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
		var f0 = Substitute.For<Func<Task<long>>>();
		var f1 = Substitute.For<Func<Task<Result<long>>>>();

		// Act
		var r0 = await cache.GetOrCreateAsync(key, f0);
		var r1 = await cache.GetOrCreateAsync(key, f1);

		// Assert
		r0.AssertOk(value);
		r1.AssertOk(value);
		_ = f0.DidNotReceive().Invoke();
		_ = f1.DidNotReceive().Invoke();
	}

	[Fact]
	public async Task Value_Does_Not_Exist__Calls_Cache_CreateEntry__Sets_Value__Returns_Value()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var mc = Miss();
		var cache = new WrapCache<string>(mc);

		// Act
		var r0 = await cache.GetOrCreateAsync(key, () => Task.FromResult(value));
		var r1 = await cache.GetOrCreateAsync(key, () => Task.FromResult(R.Wrap(value)));
		var r2 = await cache.GetOrCreateAsync(key, () => Task.FromResult(value), new());
		var r3 = await cache.GetOrCreateAsync(key, () => Task.FromResult(R.Wrap(value)), new());

		// Assert
		r0.AssertOk(value);
		r1.AssertOk(value);
		r2.AssertOk(value);
		r3.AssertOk(value);
		var entry = mc.Received(4).CreateEntry(key);
		Assert.Equal(value, entry.Value);
	}

	[Fact]
	public async Task Factory_Returns_Failure__Returns_That_Failure__Does_Not_Store()
	{
		// Arrange
		var key = Rnd.Str;
		var failure = FailGen.Create();
		var mc = Miss();
		var cache = new WrapCache<string>(mc);

		// Act
		var result = await cache.GetOrCreateAsync(key, () => Task.FromResult(FailGen.Create<long>(failure)));

		// Assert
		Assert.Equal(failure, result.AssertFailure());
		_ = mc.DidNotReceive().CreateEntry(Arg.Any<object>());
	}

	[Fact]
	public async Task Factory_Throws__Returns_Failure_With_ErrorCreatingCacheValueMsg()
	{
		// Arrange
		var key = Rnd.Str;
		var ex = new Exception(Rnd.Str);
		Task<long> f0() => throw ex;
		Task<Result<long>> f1() => throw ex;
		var cache = new WrapCache<string>(Miss());

		// Act
		var r0 = await cache.GetOrCreateAsync(key, f0);
		var r1 = await cache.GetOrCreateAsync(key, f1);

		// Assert
		r0.AssertFailure(ex, "Error creating cache value.");
		r1.AssertFailure(ex, "Error creating cache value.");
	}

	[Fact]
	public async Task Factory_Fails__Next_Call_Runs_Factory_Again()
	{
		// Arrange
		var key = Rnd.Str;
		var value = Rnd.Lng;
		var cache = new WrapCache<string>(new MemoryCache(new MemoryCacheOptions()));

		// Act
		var r0 = await cache.GetOrCreateAsync(key, () => Task.FromResult(FailGen.Create<long>()));
		var r1 = await cache.GetOrCreateAsync(key, () => Task.FromResult(R.Wrap(value)));

		// Assert
		_ = r0.AssertFailure();
		r1.AssertOk(value);
	}

	[Fact]
	public async Task Creates_Entry__Waits_For_Expiry__Creates_Again()
	{
		// Arrange
		var key = Rnd.Str;
		var v0 = Rnd.Lng;
		var v1 = Rnd.Lng;
		var cache = new WrapCache<string>(new MemoryCache(new MemoryCacheOptions()));
		var ms = 200;

		// Act
		var r0 = await cache.GetOrCreateAsync(key, () => Task.FromResult(v0), new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(ms) });
		var r1 = await cache.GetOrCreateAsync(key, () => Task.FromResult(v1));
		await Task.Delay(TimeSpan.FromMilliseconds(ms * 2), TestContext.Current.CancellationToken);
		var r2 = await cache.GetOrCreateAsync(key, () => Task.FromResult(v1));

		// Assert
		r0.AssertOk(v0);
		r1.AssertOk(v0);
		r2.AssertOk(v1);
	}

	[Fact]
	public async Task Slow_Factory__Does_Not_Block_Other_Keys()
	{
		// Arrange - key 1's factory cannot finish until the test releases it
		var cache = new WrapCache<int>(new MemoryCache(new MemoryCacheOptions()));
		var gate = new TaskCompletionSource<Result<int>>();
		var ct = TestContext.Current.CancellationToken;

		// Act - WaitAsync throws TimeoutException if key 2 waits for key 1
		var slow = cache.GetOrCreateAsync(1, () => gate.Task);
		var other = await cache.GetOrCreateAsync(2, () => Task.FromResult(2)).WaitAsync(TimeSpan.FromSeconds(5), ct);
		gate.SetResult(1);

		// Assert
		other.AssertOk(2);
		(await slow).AssertOk(1);
	}

	[Fact]
	public async Task Factory_Calls_GetOrCreateAsync_For_Another_Key__Returns_Value()
	{
		// Arrange
		var cache = new WrapCache<int>(new MemoryCache(new MemoryCacheOptions()));
		var inner = Rnd.Int;
		var ct = TestContext.Current.CancellationToken;

		// Act - WaitAsync throws TimeoutException if the call hangs
		var result = await cache.GetOrCreateAsync(1, () => cache.GetOrCreateAsync(2, () => Task.FromResult(inner)))
			.WaitAsync(TimeSpan.FromSeconds(5), ct);

		// Assert
		result.AssertOk(inner);
	}
}
