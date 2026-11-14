// Wrap: Functional Monads for .NET
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
	/// Get the only item in the list held by <paramref name="this"/>.
	/// </summary>
	/// <remarks>
	/// At most two items are read, so a long or infinite sequence is not enumerated in full.
	/// </remarks>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <typeparam name="TSingle">List item type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="onError">[Optional] Creates the <see cref="Failure"/> to return when there is not exactly one item,
	/// instead of the default failure.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> holding the only item; the <see cref="Failure"/> held by <paramref name="this"/>; or a
	/// <see cref="Failure"/> if the list is empty, has more than one item or does not hold <typeparamref name="TSingle"/>
	/// items, or if the only item is <see langword="null"/>.
	/// </returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, R.ErrorHandler? onError)
		where T : IEnumerable =>
		@this switch
		{
			Ok<T> x =>
				x.Value switch
				{
					IEnumerable<TSingle> y => y.Take(2).ToArray() switch
					{
						{ Length: 1 } z =>
							R.Wrap(z[0]),

						{ Length: 0 } =>
							onError?.Invoke(C.GetSingle.EmptyList) ?? R.Fail(C.GetSingle.EmptyList)
								.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

						_ =>
							onError?.Invoke(C.GetSingle.MultipleValues) ?? R.Fail(C.GetSingle.MultipleValues)
								.Ctx(nameof(ResultExtensions), nameof(GetSingle))
					},

					IEnumerable =>
						onError?.Invoke(C.GetSingle.IncorrectType, typeof(TSingle).Name, typeof(T).Name) ?? R.Fail(C.GetSingle.IncorrectType, typeof(TSingle).Name, typeof(T).Name)
							.Ctx(nameof(ResultExtensions), nameof(GetSingle)),

					_ =>
						onError?.Invoke(C.GetSingle.NotAList) ?? R.Fail(C.GetSingle.NotAList)
							.Ctx(nameof(ResultExtensions), nameof(GetSingle))
				},

			{ } x =>
				x.PassFailure<TSingle>()
		};

	/// <inheritdoc cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandler?)"/>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this, Func<FluentGetSingle<T>, Result<TSingle>> unwrap)
		where T : IEnumerable<TSingle> =>
		GetSingle(@this, unwrap, null);

	/// <summary>
	/// Get the only item in the list held by <paramref name="this"/>, using a fluent call so <typeparamref name="TSingle"/>
	/// can be inferred: <c>result.GetSingle(x => x.Value&lt;int&gt;())</c>.
	/// </summary>
	/// <typeparam name="T">Ok value type - a list of <typeparamref name="TSingle"/>.</typeparam>
	/// <typeparam name="TSingle">List item type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="unwrap">Function that calls <see cref="FluentGetSingle{T}.Value{TSingle}"/>.</param>
	/// <param name="onError">[Optional] Creates the <see cref="Failure"/> to return when there is not exactly one item,
	/// instead of the default failure.</param>
	/// <returns>The result returned by <paramref name="unwrap"/> - see <see cref="GetSingle{T, TSingle}(Result{T}, R.ErrorHandler?)"/>.</returns>
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
	public static Task<Result<TSingle>> GetSingleAsync<T, TSingle>(this Task<Result<T>> @this,
		Func<FluentGetSingle<T>, Result<TSingle>> unwrap, R.ErrorHandler? onError)
		where T : IEnumerable<TSingle> =>
		@this.Then(x => x.GetSingle(unwrap, onError));

	/// <summary>
	/// Fluent helper used by <see cref="GetSingle{T, TSingle}(Result{T}, Func{FluentGetSingle{T}, Result{TSingle}}, R.ErrorHandler?)"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable"/>.</typeparam>
	/// <param name="result">Result holding the list.</param>
	/// <param name="onError">[Optional] Creates the <see cref="Failure"/> to return when there is not exactly one item.</param>
	public sealed class FluentGetSingle<T>(Result<T> result, R.ErrorHandler? onError)
		where T : IEnumerable
	{
		/// <summary>
		/// Get the only item in the list - see <see cref="GetSingle{T, TSingle}(Result{T}, R.ErrorHandler?)"/>.
		/// </summary>
		/// <typeparam name="TSingle">List item type.</typeparam>
		/// <returns><see cref="Ok{T}"/> holding the only item, or a <see cref="Failure"/>.</returns>
		public Result<TSingle> Value<TSingle>() =>
			GetSingle<T, TSingle>(result, onError);
	}
}

