// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

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
		@this.Value;

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static async Task<Result<T>> ToResultAsync<T>(this IMonad<T> @this) =>
		@this.Value;

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
	public static async Task<Result<T>> ToResultAsync<T>(this Task<IMonad<T>> @this) =>
		(await @this).Value;

	/// <inheritdoc cref="ToResult{T}(IMonad{T})"/>
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
	public static async Task<IEnumerable<Result<T>>> ToResultAsync<T>(this Task<IEnumerable<IMonad<T>>> @this) =>
		ToResult(await @this);
}
