// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Keep the value of <paramref name="this"/> only if <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <seealso cref="Linq.MaybeToMaybeExtensions.Where{T}(Maybe{T}, Func{T, bool})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns><paramref name="this"/> if it is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>, otherwise <see cref="None"/>.</returns>
	public static Maybe<T> Filter<T>(this Maybe<T> @this, Func<T, bool> fTest) =>
		@this switch
		{
			Some<T> x when fTest(x.Value) =>
				x,

			_ =>
				M.None
		};

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static async Task<Maybe<T>> FilterAsync<T>(this Maybe<T> @this, Func<T, Task<bool>> fTest) =>
		@this switch
		{
			Some<T> x when await fTest(x.Value) =>
				x,

			_ =>
				M.None
		};

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static async Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest) =>
		await @this switch
		{
			Some<T> x when fTest(x.Value) =>
				x,

			_ =>
				M.None
		};

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static async Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> fTest) =>
		await @this switch
		{
			Some<T> x when await fTest(x.Value) =>
				x,

			_ =>
				M.None
		};
}
