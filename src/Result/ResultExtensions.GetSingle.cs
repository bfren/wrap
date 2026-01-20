// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
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
	/// <typeparam name="T">Ok value type - limited to <see cref="IEnumerable{TSingle}"/>.</typeparam>
	/// <typeparam name="TSingle">IEnumerable value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>The single value contained in <paramref name="this"/>, or <see cref="Fail"/></returns>
	public static Result<TSingle> GetSingle<T, TSingle>(this Result<T> @this) =>
		Bind(@this, x => x switch
		{
			IEnumerable<TSingle> list when list.Count() == 1 =>
				R.Wrap(list.Single()),

			IEnumerable<TSingle> list when !list.Any() =>
				R.Fail(nameof(ResultExtensions), nameof(GetSingle),
					"Cannot get single value from an empty list."
				),

			IEnumerable<TSingle> =>
				R.Fail(nameof(ResultExtensions), nameof(GetSingle),
					"Cannot get single value from a list with multiple values."
				),

			IEnumerable =>
				R.Fail(nameof(ResultExtensions), nameof(GetSingle),
					"Cannot get single value from a list with incompatible value type."
				),

			_ =>
				R.Fail(nameof(ResultExtensions), nameof(GetSingle),
					"Result value type is not a list."
				)
		});

	/// <summary>
	/// Asynchronously projects a single element from the result of a task that yields a collection, using a specified
	/// selector function.
	/// </summary>
	/// <remarks>Use this method to extract or transform a single element from a collection result in an
	/// asynchronous workflow. The selector function can be used to specify which element to retrieve or how to handle
	/// cases such as empty or multiple elements.</remarks>
	/// <typeparam name="T">The type of the collection contained in the result.</typeparam>
	/// <typeparam name="TSingle">The type of the single element to be projected from the collection.</typeparam>
	/// <param name="this">A task that, when completed, provides a result containing a collection of elements.</param>
	/// <param name="unwrap">A function that defines how to select a single element from the provided collection result.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a result wrapping the single projected
	/// element.</returns>
	public static async Task<Result<TSingle>> GetSingleAsync<T, TSingle>(this Task<Result<T>> @this, Func<FluentGetSingle<T>, Result<TSingle>> unwrap)
		where T : IEnumerable<TSingle> =>
		unwrap(new FluentGetSingle<T>(await @this.ConfigureAwait(false)));

	/// <summary>
	/// Provides a fluent API for retrieving a single result value of type <typeparamref name="T"/> from a query operation.
	/// </summary>
	/// <typeparam name="T">The type of the value to retrieve from the query result.</typeparam>
	public sealed class FluentGetSingle<T>(Result<T> result)
	{
		/// <summary>
		/// Retrieves the result value as an instance of the specified type.
		/// </summary>
		/// <typeparam name="TSingle">The type to which the result value is converted.</typeparam>
		/// <returns>A <see cref="Result{TSingle}"/> containing the result value converted to <typeparamref name="TSingle"/>.</returns>
		public Result<TSingle> Value<TSingle>() =>
			GetSingle<T, TSingle>(result);
	}
}

