// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, turning <see cref="None"/> into a <see cref="Failure"/>.
	/// </summary>
	/// <remarks>
	/// The async overload also returns a <see cref="Failure"/> holding the exception if awaiting <paramref name="this"/> throws.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="class">Failure context class.</param>
	/// <param name="function">Failure context function.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value, or a <see cref="Failure"/> with the message <c>Maybe&lt;{Type}&gt; was 'None'.</c>
	/// and the context <c>{class}.{function}()</c> if <paramref name="this"/> is <see cref="None"/>.
	/// </returns>
	public static Result<T> ToResult<T>(this Maybe<T> @this, string @class, string function) =>
		@this switch
		{
			Some<T> x =>
				x.Value,

			None =>
				R.Fail(C.NoneFailureMessage, typeof(T).Name).Ctx(@class, function)
		};

	/// <inheritdoc cref="ToResult{T}(Maybe{T}, string, string)"/>
	public static async Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, string @class, string function)
	{
		try
		{
			return ToResult(await @this, @class, function);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error converting Maybe<{Type}> to Result.", typeof(T).Name);
		}
	}

	/// <summary>
	/// Convert a <see cref="Maybe{T}"/> to a <see cref="Result{T}"/>, using <paramref name="noneHandler"/> when it is <see cref="None"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="noneHandler">Returns the result to use when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the value, the result returned by <paramref name="noneHandler"/>, or a <see cref="Failure"/>
	/// holding the exception if <paramref name="noneHandler"/> (or awaiting <paramref name="this"/>) throws.
	/// </returns>
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
	public static async Task<Result<T>> ToResultAsync<T>(this Task<Maybe<T>> @this, Func<Result<T>> noneHandler)
	{
		try
		{
			return ToResult(await @this, noneHandler);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Msg("Error converting Maybe<{Type}> to Result.", typeof(T).Name);
		}
	}

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
