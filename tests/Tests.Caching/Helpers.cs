// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Caching;

public static class Helpers
{
	public static void AssertFailure<T>(this IWrapCache<T> @this, string message, params object?[] args)
		where T : notnull =>
		@this.LastFailure.AssertSome().Value.AssertMessage(message, [.. args]);

	public static void AssertFailure<T>(this IWrapCache<T> @this, Exception ex, string message, params object?[] args)
		where T : notnull =>
		@this.LastFailure.AssertSome().Value.AssertException(ex).AssertMessage(message, [.. args]);
}
