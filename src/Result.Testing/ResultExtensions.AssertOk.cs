// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Ok{T}"/> and return the wrapped value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>The value of <paramref name="this"/>.</returns>
	public static T AssertOk<T>(this Result<T> @this) =>
		Assert.IsType<Ok<T>>(@this).Value;

	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Ok{T}"/> and the wrapped value matches <paramref name="expected"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="expected">Expected value.</param>
	/// <returns>The value of <paramref name="this"/>.</returns>
	public static void AssertOk<T>(this Result<T> @this, T expected)
	{
		var value = Assert.IsType<Ok<T>>(@this).Value;
		Assert.Equal(expected, value);
	}
}
