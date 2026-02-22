// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	#region Values

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static Monad<T> Wrap<T>(this T @this) =>
		F.Wrap<Monad<T>, T>(@this);

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static Task<Monad<T>> WrapAsync<T>(this T @this) =>
		Task.FromResult(Wrap(@this));

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static async Task<Monad<T>> WrapAsync<T>(this Task<T> @this) =>
		Wrap(await @this);

	#endregion

	#region IEnumerable

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static IEnumerable<Monad<T>> Wrap<T>(this IEnumerable<T> @this)
	{
		foreach (var item in @this)
		{
			if (item is not null)
			{
				yield return F.Wrap<Monad<T>, T>(item);
			}
		}
	}

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static Task<IEnumerable<Monad<T>>> WrapAsync<T>(this IEnumerable<T> @this) =>
		Task.FromResult(Wrap(@this));

	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static async Task<IEnumerable<Monad<T>>> WrapAsync<T>(this Task<IEnumerable<T>> @this) =>
		Wrap(await @this);

	#endregion
}
