// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	#region Values

	/// <summary>
	/// Wrap a value in a <see cref="Monad{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Value to wrap.</param>
	/// <returns>A <see cref="Monad{T}"/> holding <paramref name="this"/>.</returns>
	/// <exception cref="Exceptions.NullMonadValueException"><paramref name="this"/> is <see langword="null"/> and <typeparamref name="T"/> is not a nullable value type.</exception>
	public static Monad<T> Wrap<T>(this T @this) =>
		F.Wrap<Monad<T>, T>(@this);

	/// <inheritdoc cref="Wrap{T}(T)"/>
	/// <returns>A completed task holding a <see cref="Monad{T}"/> with <paramref name="this"/>.</returns>
	public static Task<Monad<T>> WrapAsync<T>(this T @this) =>
		Task.FromResult(Wrap(@this));

	/// <summary>
	/// Await a value and wrap it in a <see cref="Monad{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Task returning the value to wrap.</param>
	/// <returns>A <see cref="Monad{T}"/> holding the awaited value.</returns>
	/// <exception cref="Exceptions.NullMonadValueException">The awaited value is <see langword="null"/> and <typeparamref name="T"/> is not a nullable value type.</exception>
	public static async Task<Monad<T>> WrapAsync<T>(this Task<T> @this) =>
		Wrap(await @this);

	#endregion

	#region IEnumerable

	/// <summary>
	/// Wrap each value in a sequence in a <see cref="Monad{T}"/>, skipping <see langword="null"/> values.
	/// </summary>
	/// <remarks>
	/// The sequence is read lazily, as the result is enumerated.
	/// </remarks>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Values to wrap.</param>
	/// <returns>One <see cref="Monad{T}"/> for each value in <paramref name="this"/> that is not <see langword="null"/>.</returns>
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

	/// <inheritdoc cref="Wrap{T}(IEnumerable{T})"/>
	/// <returns>A completed task holding one <see cref="Monad{T}"/> for each value in <paramref name="this"/> that is not <see langword="null"/>.</returns>
	public static Task<IEnumerable<Monad<T>>> WrapAsync<T>(this IEnumerable<T> @this) =>
		Task.FromResult(Wrap(@this));

	/// <summary>
	/// Await a sequence and wrap each value in a <see cref="Monad{T}"/>, skipping <see langword="null"/> values.
	/// </summary>
	/// <remarks>
	/// The awaited sequence is read lazily, as the result is enumerated.
	/// </remarks>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Task returning the values to wrap.</param>
	/// <returns>One <see cref="Monad{T}"/> for each awaited value that is not <see langword="null"/>.</returns>
	public static async Task<IEnumerable<Monad<T>>> WrapAsync<T>(this Task<IEnumerable<T>> @this) =>
		Wrap(await @this);

	#endregion
}
