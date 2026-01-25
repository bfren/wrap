// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Wraps <paramref name="this"/> in an <see cref="Unsafe{T}(Result{T})"/> to enable unsafe functions.
	/// </summary>
	/// <param name="this">Result object.</param>
	/// <returns>Unsafe wrapper containing <paramref name="this"/>.</returns>
	public static Unsafe<T> Unsafe<T>(this Result<T> @this) =>
		new() { Result = @this };

	/// <inheritdoc cref="Unsafe{T}(Result{T})"/>
	public static async Task<Unsafe<T>> Unsafe<T>(this Task<Result<T>> @this) =>
		new() { Result = await @this };
}
