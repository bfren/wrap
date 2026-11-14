// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MonadExtensions
{
	/// <summary>
	/// Convert an <see cref="IMonad{T}"/> to a <see cref="Result{T}"/> holding its value.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Monad object.</param>
	/// <returns><see cref="Ok{T}"/> holding the value of <paramref name="this"/>, or a <see cref="Failure"/> if the value is <see langword="null"/>.</returns>
	public static Result<T> ToResult<T>(this IMonad<T> @this) =>
		@this.Value;

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static async Task<Result<T>> ToResultAsync<T>(this IMonad<T> @this) =>
		@this.Value;

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<IMonad<T>> @this) =>
		@this.Then(x => x.ToResult());

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	/// <returns>One <see cref="Result{T}"/> for each monad in <paramref name="this"/> that is not <see langword="null"/>.</returns>
	public static IEnumerable<Result<T>> ToResult<T>(this IEnumerable<IMonad<T>> @this)
	{
		foreach (var item in @this)
		{
			if (item is not null)
			{
				yield return item.Value;
			}
		}
	}

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static Task<IEnumerable<Result<T>>> ToResultAsync<T>(this IEnumerable<IMonad<T>> @this) =>
		Task.FromResult(ToResult(@this));

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static Task<IEnumerable<Result<T>>> ToResultAsync<T>(this Task<IEnumerable<IMonad<T>>> @this) =>
		@this.Then(x => x.ToResult());
}
