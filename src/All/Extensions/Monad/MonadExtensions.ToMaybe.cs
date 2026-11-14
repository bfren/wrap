// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MonadExtensions
{
	/// <summary>
	/// Convert an <see cref="IMonad{T}"/> to a <see cref="Maybe{T}"/> holding its value.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns><see cref="Some{T}"/> holding the value of <paramref name="this"/>, or <see cref="None"/> if the value is <see langword="null"/>.</returns>
	public static Maybe<T> ToMaybe<T>(this IMonad<T> @this) =>
		@this.Value;

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static async Task<Maybe<T>> ToMaybeAsync<T>(this IMonad<T> @this) =>
		@this.Value;

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static Task<Maybe<T>> ToMaybeAsync<T>(this Task<IMonad<T>> @this) =>
		@this.Then(x => x.ToMaybe());

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	/// <returns>One <see cref="Maybe{T}"/> for each monad in <paramref name="this"/> that is not <see langword="null"/>.</returns>
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
	public static async Task<IEnumerable<Maybe<T>>> ToMaybeAsync<T>(this IEnumerable<IMonad<T>> @this) =>
		ToMaybe(@this);

	/// <inheritdoc cref="ToMaybe{T}(IMonad{T})"/>
	public static Task<IEnumerable<Maybe<T>>> ToMaybeAsync<T>(this Task<IEnumerable<IMonad<T>>> @this) =>
		@this.Then(x => x.ToMaybe());
}
