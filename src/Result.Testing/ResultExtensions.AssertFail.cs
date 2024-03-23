// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Result{T}.Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>The failure value of <paramref name="this"/>.</returns>
	public static FailValue AssertFail<T>(this Result<T> @this) =>
		Assert.IsType<Result<T>.Failure>(@this).Value;
}
