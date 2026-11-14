// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Linq;

public static partial class MaybeToResultExtensions
{
	internal static Result<TReturn> ConvertNoneToFail<T, TReturn>() =>
		R.Fail(C.NoneFailureMessage, typeof(T).Name);

	/// <summary>
	/// Convert the value of a <see cref="Maybe{T}"/> in a LINQ query, producing a <see cref="Result{T}"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// Result&lt;int&gt; r = from x in maybe
	///                    select x * 2;
	/// </code>
	/// <para>
	/// Exceptions thrown by <paramref name="f"/> are not caught.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="f">Function that converts the value.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="f"/>; a <see cref="Failure"/> with the message
	/// <c>Maybe&lt;{Type}&gt; was 'None'.</c> if <paramref name="this"/> is <see cref="None"/>; or a <see cref="Failure"/>
	/// if <paramref name="f"/> returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> Select<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> f) =>
		@this switch
		{
			Some<T> x =>
				f(x.Value),

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> Select<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> f) =>
		@this switch
		{
			Some<T> x =>
				await f(x.Value),

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> f) =>
		await @this switch
		{
			Some<T> x =>
				f(x.Value),

			None =>
				ConvertNoneToFail<T, TReturn>()
		};

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static async Task<Result<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		await @this switch
		{
			Some<T> x =>
				await f(x.Value),

			None =>
				ConvertNoneToFail<T, TReturn>()
		};
}
