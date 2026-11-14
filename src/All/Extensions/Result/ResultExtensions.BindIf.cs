// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> with the value when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="f">Function that converts the value to a <see cref="Result{T}"/>, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>
	/// The <see cref="Result{T}"/> returned by <paramref name="f"/>; the <see cref="Failure"/> held by <paramref name="this"/>;
	/// or a <see cref="Failure"/> with the message <c>Test returned false.</c> if <paramref name="fTest"/> returns <see langword="false"/>.
	/// </returns>
	public static Result<TReturn> BindIf<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> f) =>
		If(@this, fTest, f, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(BindIf)));

	/// <inheritdoc cref="BindIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindIfAsync<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> f) =>
		IfAsync(@this, fTest, f, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(BindIfAsync)));

	/// <inheritdoc cref="BindIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindIfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> f) =>
		IfAsync(@this, fTest, f, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(BindIfAsync)));

	/// <inheritdoc cref="BindIf{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}})"/>
	public static Task<Result<TReturn>> BindIfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> f) =>
		IfAsync(@this, fTest, f, _ => R.Fail(C.TestFalseMessage).Ctx(nameof(ResultExtensions), nameof(BindIfAsync)));
}
