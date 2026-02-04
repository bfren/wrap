// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class MonadExtensions
{
	/// <summary>
	/// Convert a <see cref="IMonad{T}"/> to <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this IMonad<T> @this) =>
		R.Wrap(@this.Value);
}
