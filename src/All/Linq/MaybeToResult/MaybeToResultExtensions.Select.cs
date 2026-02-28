// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeToResultExtensions
{
	internal static Result<TReturn> ConvertNoneToFail<T, TReturn>() =>
		R.Fail(C.NoneFailureMessage, typeof(T).Name);

	/// <summary>
	/// Enables LINQ Select() to convert <see cref="Maybe{T}"/> objects to <see cref="Result{T}"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Maybe
	/// select x
	/// </code>
	/// <para>
	/// Returns value of x if <see cref="Maybe{T}"/> object is <see cref="Some{T}"/>,
	/// and <see cref="None"/> if not.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Maybe type.</typeparam>
	/// <typeparam name="TReturn">Return type.</typeparam>
	/// <param name="this">Maybe.</param>
	/// <param name="f">Return map function.</param>
	/// <returns>Result object.</returns>
	public static Result<TReturn> Select<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> f) =>
		M.Match(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: x => f(x)
		);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> f) =>
		M.MatchAsync<T, Result<TReturn>>(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: async x => (await f(x)).Wrap()
		);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> f) =>
		M.MatchAsync<T, Result<TReturn>>(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: async x => f(x).Wrap()
		);

	/// <inheritdoc cref="Select{T, TReturn}(Maybe{T}, Func{T, TReturn})"/>
	public static Task<Result<TReturn>> Select<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> f) =>
		M.MatchAsync<T, Result<TReturn>>(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: async x => (await f(x)).Wrap()
		);
}
