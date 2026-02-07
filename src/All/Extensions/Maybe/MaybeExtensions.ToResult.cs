// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, converting <see cref="None"/>
	/// values to a <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, string @class, string function) =>
		ToResult(@this, () => R.Fail(C.NoneFailureMessage, typeof(T).Name).Ctx(@class, function));

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, string, string)"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, string @class, string function) =>
		ToResultAsync(@this, async () => R.Fail(C.NoneFailureMessage, typeof(T).Name).Ctx(@class, function));

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">Value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="noneHandler">What to do when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>Result object.</returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, Func<Result<T>> noneHandler)
	{
		try
		{
			return M.Match(@this,
				fNone: noneHandler,
				fSome: R.Wrap
			);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error converting Maybe<{Type}> to Result.", typeof(T).Name);
		}
	}

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Result<T>> noneHandler) =>
		ToResultAsync(@this, async () => noneHandler());

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, Func{Result{T}})"/>
	public static async Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Task<Result<T>>> noneHandler)
	{
		try
		{
			return await M.MatchAsync(@this,
				fNone: noneHandler,
				fSome: R.Wrap
			);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error converting Maybe<{Type}> to Result.", typeof(T).Name);
		}
	}
}
