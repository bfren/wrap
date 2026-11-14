// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Keep the value of <paramref name="this"/> only if <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <seealso cref="Filter{T}(Result{T}, Func{T, bool})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <returns>
	/// <paramref name="this"/> if it is a <see cref="Failure"/> or <paramref name="fTest"/> returns <see langword="true"/>,
	/// otherwise a <see cref="Failure"/> with the message <c>Test returned false.</c>.
	/// </returns>
	public static Result<T> ContinueIf<T>(this Result<T> @this, Func<T, bool> fTest) =>
		If<T, T>(@this, fTest, x => x, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(ContinueIf)));

	/// <inheritdoc cref="ContinueIf{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> ContinueIfAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest) =>
		IfAsync<T, T>(@this, fTest, x => x, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(ContinueIfAsync)));

}
