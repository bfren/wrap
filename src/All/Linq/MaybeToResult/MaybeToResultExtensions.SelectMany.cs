// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Extensions;

namespace Wrap.Linq;

public static partial class MaybeToResultExtensions
{
	/// <summary>
	/// Enables LINQ SelectMany() to convert <see cref="Maybe{T}"/> objects to <see cref="Result{T}"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// from x in Maybe
	/// from y in Maybe
	/// select x + y
	/// </code>
	/// <para>
	/// Returns value of x + y if both <see cref="Maybe{T}"/> objects are <see cref="Some{T}"/>,
	/// and <see cref="None"/> if either is <see cref="None"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Maybe type.</typeparam>
	/// <typeparam name="TInner">Interim type.</typeparam>
	/// <typeparam name="TReturn">Return type.</typeparam>
	/// <param name="this">Maybe.</param>
	/// <param name="f">Interim bind function.</param>
	/// <param name="g">Return map function.</param>
	/// <returns>Result object.</returns>
	public static Result<TReturn> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		M.Match(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: x => f(x).Map(y => g(x, y))
		);

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Maybe<T> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		M.MatchAsync(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: x => f(x).MapAsync(y => g(x, y))
		);

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Result<TInner>> f, Func<T, TInner, TReturn> g) =>
		M.MatchAsync(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: x => f(x).Map(y => g(x, y))
		);

	/// <inheritdoc cref="SelectMany{T, TInner, TReturn}(Maybe{T}, Func{T, Result{TInner}}, Func{T, TInner, TReturn})"/>
	public static Task<Result<TReturn>> SelectMany<T, TInner, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Result<TInner>>> f, Func<T, TInner, TReturn> g) =>
		M.MatchAsync(@this,
			fNone: ConvertNoneToFail<T, TReturn>,
			fSome: x => f(x).MapAsync(y => g(x, y))
		);
}
