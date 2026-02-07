// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class ResultExtensions
{
	/// <summary>
	/// Enables LINQ Where() on <see cref="Result{T}"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Result
	/// where x == y
	/// select x
	/// </code>
	/// <para>
	/// Returns value of x if <see cref="Result{T}"/> object is <see cref="Ok{T}"/> and x is equal to y,
	/// and <see cref="Failure"/> if not.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Select where fTest.</param>
	public static Result<T> Where<T>(this Result<T> @this, Func<T, bool> fTest) =>
		@this.Filter(fTest);

	/// <inheritdoc cref="Where{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> Where<T>(this Result<T> @this, Func<T, Task<bool>> fTest) =>
		@this.FilterAsync(fTest);

	/// <inheritdoc cref="Where{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> Where<T>(this Task<Result<T>> @this, Func<T, bool> fTest) =>
		@this.FilterAsync(fTest);

	/// <inheritdoc cref="Where{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> Where<T>(this Task<Result<T>> @this, Func<T, Task<bool>> fTest) =>
		@this.FilterAsync(fTest);
}
