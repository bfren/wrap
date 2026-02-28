// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Return a value determined by the value of <paramref name="this"/> and the result of <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Uses value of <paramref name="this"/> to determines whether to pass on the original value or return <see cref="None"/>.</param>
	/// <returns>Original value or <see cref="None"/>.</returns>
	public static Result<T> ContinueIf<T>(this Result<T> @this, Func<T, bool> fTest) =>
		If<T, T>(@this, fTest, x => x, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(ContinueIf)));

	/// <inheritdoc cref="ContinueIf{T}(Result{T}, Func{T, bool})"/>
	public static Task<Result<T>> ContinueIfAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest) =>
		IfAsync<T, T>(@this, fTest, x => x, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(ContinueIfAsync)));

}
