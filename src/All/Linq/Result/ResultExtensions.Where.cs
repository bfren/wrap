// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class ResultExtensions
{
	/// <summary>
	/// Filter a <see cref="Result{T}"/> in a LINQ query - see <see cref="Extensions.ResultExtensions.Filter{T}(Result{T}, Func{T, bool})"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in result
	/// where x > 0
	/// select x
	/// </code>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns>
	/// <paramref name="this"/> if it is a <see cref="Failure"/> or <paramref name="fTest"/> returns <see langword="true"/>,
	/// otherwise a <see cref="Failure"/> with the message <c>Test returned false.</c>.
	/// </returns>
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
