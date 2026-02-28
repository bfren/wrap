// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="fTest"/> when <paramref name="this"/> is <see cref="Some{T}"/>
	/// </summary>
	/// <seealso cref="Linq.MaybeToMaybeExtensions.Where{T}(Maybe{T}, Func{T, bool})"/>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Function to detemine whether or not the value of <paramref name="this"/> should be returned.</param>
	/// <returns>Value of <paramref name="this"/> if <paramref name="fTest"/> returns true, or <see cref="None"/>.</returns>
	public static Maybe<T> Filter<T>(this Maybe<T> @this, Func<T, bool> fTest) =>
		@this.Bind(x => fTest(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Maybe<T> @this, Func<T, Task<bool>> fTest) =>
		@this.BindAsync(async x => await fTest(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest) =>
		@this.BindAsync(x => fTest(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	/// <inheritdoc cref="Filter{T}(Maybe{T}, Func{T, bool})"/>
	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> fTest) =>
		@this.BindAsync(async x => await fTest(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});
}
