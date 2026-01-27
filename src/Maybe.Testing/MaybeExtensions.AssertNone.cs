// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Maybe{T}.None"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	public static void AssertNone<T>(this Maybe<T> @this) =>
		Assert.IsType<Maybe<T>.None>(@this);
}
