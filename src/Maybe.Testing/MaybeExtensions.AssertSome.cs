// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Some{T}"/> and return the wrapped value.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <returns>Some Value.</returns>
	public static T AssertSome<T>(this Maybe<T> @this) =>
		Assert.IsType<Some<T>>(@this).Value;

	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Some{T}"/> and matches <paramref name="expected"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="expected">Expected value.</param>
	public static void AssertSome<T>(this Maybe<T> @this, T expected) =>
		Assert.Equal(expected, AssertSome(@this));
}
