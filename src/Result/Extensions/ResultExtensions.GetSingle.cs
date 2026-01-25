// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	private const string EmptyList =
		"Cannot get single value from an empty list.";

	private const string MultipleValues =
		"Cannot get single value from a list with multiple values.";

	private const string IncorrectType =
		"Cannot get single value of type '{ValueType}' from a list type '{ListType}'.";

	private const string NotAList =
		"Result value type is not a list.";

	/// <summary>
	/// Unwrap an <see cref="IEnumerable{T}"/> object that has a single value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="this"/> is not an <see cref="IEnumerable{T}"/> with a single value,
	/// you will get an <see cref="InvalidOperationException"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Unwrap{T}(Result{T})"/>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <typeparam name="TSingle">IEnumerable value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	/// <returns>The single value contained in <paramref name="this"/>, or <see cref="Fail"/></returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, R.ErrorHandlerWithMsg? onError = null)
		where T : IEnumerable =>
		Bind(@this, x => x switch
		{
			IEnumerable<TSingle> list when list.Count() == 1 =>
				R.Wrap(list.Single()),

			IEnumerable<TSingle> list when !list.Any() =>
				onError?.Invoke(EmptyList) ?? R.Fail(EmptyList)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			IEnumerable<TSingle> =>
				onError?.Invoke(MultipleValues) ?? R.Fail(MultipleValues)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			IEnumerable =>
				onError?.Invoke(IncorrectType, typeof(TSingle).Name, typeof(T).Name) ?? R.Fail(IncorrectType, typeof(TSingle).Name, typeof(T).Name)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

			_ =>
				onError?.Invoke(NotAList) ?? R.Fail(NotAList)
					.Ctx(nameof(ResultExtensions), nameof(GetSingle))
		});

	/// <summary>
	/// Unwrap an <see cref="IEnumerable{T}"/> object that has a single value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="this"/> is not an <see cref="IEnumerable{T}"/> with a single value,
	/// you will get an <see cref="InvalidOperationException"/>.
	/// </para>
	/// </remarks>
	/// <seealso cref="Unwrap{T}(Result{T})"/>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <typeparam name="TSingle">IEnumerable value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="unwrap">Fluent unwrap function.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	/// <returns>The single value contained in <paramref name="this"/>, or <see cref="Fail"/></returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, Func<FluentGetSingle<T>,
		Result<TSingle>> unwrap, R.ErrorHandlerWithMsg? onError = null
	)
		where T : IEnumerable<TSingle> =>
		unwrap(new FluentGetSingle<T>(@this, onError));

	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandlerWithMsg?)"/>
	public static async Task<Result<TSingle>> GetSingleAsync<T, TSingle>(this Task<Result<T>> @this,
		Func<FluentGetSingle<T>, Result<TSingle>> unwrap, R.ErrorHandlerWithMsg? onError = null)
		where T : IEnumerable<TSingle> =>
		unwrap(new FluentGetSingle<T>(await @this, onError));

	/// <summary>
	/// Provides a fluent interface for retrieving a single value from a result containing an enumerable type.
	/// </summary>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <param name="result">The result object containing the enumerable value to be processed.</param>
	/// <param name="onError">[Optional] Return custom error on failure.</param>
	public sealed class FluentGetSingle<T>(Result<T> result, R.ErrorHandlerWithMsg? onError = null)
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

