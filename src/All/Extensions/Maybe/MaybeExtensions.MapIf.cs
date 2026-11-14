// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Convert the value with <paramref name="f"/> when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="f">Function that converts the value, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>
	/// <see cref="Some{T}"/> holding the value returned by <paramref name="f"/>, or <see cref="None"/> if <paramref name="this"/>
	/// is <see cref="None"/>, <paramref name="fTest"/> returns <see langword="false"/> or <paramref name="f"/> returns <see langword="null"/>.
	/// </returns>
	public static Maybe<TReturn> MapIf<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, TReturn> f) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					true =>
						f(x.Value),

					false =>
						M.None
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapIfAsync<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<TReturn>> f) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					true =>
						await f(x.Value),

					false =>
						M.None
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapIfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, TReturn> f) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					true =>
						f(x.Value),

					false =>
						M.None
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="MapIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, TReturn})"/>
	public static async Task<Maybe<TReturn>> MapIfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<TReturn>> f) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					true =>
						await f(x.Value),

					false =>
						M.None
				},

			None =>
				M.None
		};
}
