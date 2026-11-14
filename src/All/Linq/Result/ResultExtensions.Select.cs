// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class ResultExtensions
{
	/// <summary>
	/// Convert the value of a <see cref="Result{T}"/> in a LINQ query.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in result
	/// select x
	/// </code>
	/// <para>
	/// This calls <see cref="Extensions.ResultExtensions.Map{T, TReturn}(Result{T}, Func{T, TReturn})"/>, so an exception
	/// thrown by <paramref name="f"/> becomes a <see cref="Failure"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Function that converts the value.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value returned by <paramref name="f"/>; the <see cref="Failure"/> held by
	/// <paramref name="this"/>; or a <see cref="Failure"/> if <paramref name="f"/> throws or returns <see langword="null"/>.
	/// </returns>
	public static Result<TReturn> Select<T, TReturn>(this Result<T> @this, Func<T, TReturn> f) =>
		@this.Map(f);

	/// <inheritdoc cref="Select{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> f) =>
		@this.MapAsync(f);

	/// <inheritdoc cref="Select{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> f) =>
		@this.MapAsync(f);

	/// <inheritdoc cref="Select{T, TReturn}(Result{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> f) =>
		@this.MapAsync(f);
}
