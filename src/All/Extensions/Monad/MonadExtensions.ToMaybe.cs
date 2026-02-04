// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class MonadExtensions
{
	/// <summary>
	/// Convert a <see cref="IMonad{T}"/> to <see cref="Maybe{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<T> ToMaybe<T>(this IMonad<T> @this) =>
		M.Wrap(@this.Value);
}
