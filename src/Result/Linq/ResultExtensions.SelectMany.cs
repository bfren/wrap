// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monads.Linq;

public static partial class ResultExtensions
{
	/// <summary>
	/// Enables LINQ SelectMany() on <see cref="Result{T}"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Result
	/// from y in Result
	/// select x + y
	/// </code>
	/// <para>
	/// Returns value of x + y if both <see cref="Result{T}"/> objects are <see cref="Ok{T}"/>,
	/// and <see cref="Err"/> if either is <see cref="Err"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <typeparam name="TInner">Interim value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="f">Interim bind function.</param>
	/// <param name="g">Return map function.</param>
	public static Result<TReturn> SelectMany<T, TInner, TReturn>(this Result<T> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this.Bind(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Result<T> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Result<T>> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Result{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		@this.BindAsync(x => from y in f(x) select g(x, y));
}
