// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, R.ErrorHandler?)"/>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this)
		where T : IEnumerable =>
		GetSingle<T, TSingle>(@this, null);

	/// <summary>
	/// Unwrap an <see cref="IEnumerable{T}"/> object that has a single value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="this"/> is not an <see cref="IEnumerable{T}"/> with a single value,
	/// you will get an <see cref="InvalidOperationException"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Result{T}.Unwrap(Func{FailureValue, T})"/>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <typeparam name="TSingle">IEnumerable value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	/// <returns>The single value contained in <paramref name="this"/>, or <see cref="Failure"/></returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, R.ErrorHandler? onError)
		where T : IEnumerable =>
		Bind(@this, x => x switch
		{
			IEnumerable<TSingle> list when list.Count() == 1 =>
				R.Wrap(list.Single()),

			IEnumerable<TSingle> list when !list.Any() =>
				onError?.Invoke(C.GetSingle.EmptyList) ?? R.Fail(C.GetSingle.EmptyList)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			IEnumerable<TSingle> =>
				onError?.Invoke(C.GetSingle.MultipleValues) ?? R.Fail(C.GetSingle.MultipleValues)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			IEnumerable =>
				onError?.Invoke(C.GetSingle.IncorrectType, typeof(TSingle).Name, typeof(T).Name) ?? R.Fail(C.GetSingle.IncorrectType, typeof(TSingle).Name, typeof(T).Name)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			_ =>
				onError?.Invoke(C.GetSingle.NotAList) ?? R.Fail(C.GetSingle.NotAList)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle))
		});

	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandler?)"/>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, Func<FluentGetSingle<T>, Result<TSingle>> unwrap)
		where T : IEnumerable<TSingle> =>
		GetSingle(@this, unwrap, null);

	/// <summary>
	/// Unwrap an <see cref="IEnumerable{T}"/> object that has a single value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="this"/> is not an <see cref="IEnumerable{T}"/> with a single value,
	/// you will get an <see cref="InvalidOperationException"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Result{T}.Unwrap(Func{FailureValue, T})"/>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <typeparam name="TSingle">IEnumerable value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="unwrap">Fluent unwrap function.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	/// <returns>The single value contained in <paramref name="this"/>, or <see cref="Failure"/></returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, Func<FluentGetSingle<T>,
		Result<TSingle>> unwrap, R.ErrorHandler? onError
	)
		where T : IEnumerable<TSingle> =>
		unwrap(new FluentGetSingle<T>(@this, onError));

	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandler?)"/>
	public static Task<Result<TSingle>> GetSingleAsync<T, TSingle>(this Task<Result<T>> @this,
		Func<FluentGetSingle<T>, Result<TSingle>> unwrap)
		where T : IEnumerable<TSingle> =>
		GetSingleAsync(@this, unwrap, null);

	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandler?)"/>
	public static async Task<Result<TSingle>> GetSingleAsync<T, TSingle>(this Task<Result<T>> @this,
		Func<FluentGetSingle<T>, Result<TSingle>> unwrap, R.ErrorHandler? onError)
		where T : IEnumerable<TSingle> =>
		unwrap(new FluentGetSingle<T>(await @this, onError));

	/// <summary>
	/// Provides a fluent interface for retrieving a single value from a result containing an enumerable type.
	/// </summary>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <param name="result">The result object containing the enumerable value to be processed.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	public sealed class FluentGetSingle<T>(Result<T> result, R.ErrorHandler? onError)
		where T : IEnumerable
	{
		/// <summary>
		/// Retrieves the result value as an instance of the specified type.
		/// </summary>
		/// <typeparam name="TSingle">The type to which the result value is converted.</typeparam>
		/// <returns>A <see cref="Result{TSingle}"/> containing the result value converted to <typeparamref name="TSingle"/>.</returns>
		public Result<TSingle> Value<TSingle>() =>
			GetSingle<T, TSingle>(result, onError);
	}
}

