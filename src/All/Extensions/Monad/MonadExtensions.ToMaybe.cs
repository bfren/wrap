// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

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
		@this.Value;

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this IMonad<T> @this) =>
		M.Wrap(@this.Value).AsTask();

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this Task<IMonad<T>> @this) =>
		(await @this).Value;

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static IEnumerable<Maybe<T>> ToMaybe<T>(this IEnumerable<IMonad<T>> @this)
	{
		foreach (var item in @this)
		{
			if (item is not null)
			{
				yield return item.Value;
			}
		}
	}

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static Task<IEnumerable<Maybe<T>>> ToMaybeAsync<T>(this IEnumerable<IMonad<T>> @this) =>
		Task.FromResult(ToMaybe(@this));

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static async Task<IEnumerable<Maybe<T>>> ToMaybeAsync<T>(this Task<IEnumerable<IMonad<T>>> @this) =>
		ToMaybe(await @this);
}
