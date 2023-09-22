// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Result{T}.Err"/>.
	/// </summary>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="this">Result object.</param>
	public static void AssertErr<T>(this Result<T> @this) =>
		Assert.IsType<Result<T>.Err>(@this);
}