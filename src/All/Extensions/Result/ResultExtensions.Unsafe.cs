// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Wrap <paramref name="this"/> in an <see cref="Unsafe{TEither, TLeft, TRight}"/> object, to use the unsafe functions.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>An <see cref="Unsafe{TEither, TLeft, TRight}"/> object wrapping <paramref name="this"/>.</returns>
	public static Unsafe<Result<T>, Failure, T> Unsafe<T>(this Result<T> @this) =>
		new(@this);

	/// <inheritdoc cref="Unsafe{T}(Result{T})"/>
	public static async Task<Unsafe<Result<T>, Failure, T>> Unsafe<T>(this Task<Result<T>> @this) =>
		new() { Value = await @this };
}
