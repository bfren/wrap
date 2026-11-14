// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="fFail"/>, <paramref name="fFalse"/> or <paramref name="fTrue"/>, depending on the value of
	/// <paramref name="this"/> and the result of <paramref name="fTest"/>, and return its value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Run with the failure when <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fFalse">Run with the value when <paramref name="this"/> is an <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <param name="fTrue">Run with the value when <paramref name="this"/> is an <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>Value returned by <paramref name="fFail"/>, <paramref name="fFalse"/> or <paramref name="fTrue"/>.</returns>
	public static TReturn MatchIf<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, TReturn> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Result<T> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Result{T}, Func{Failure, TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Result<T>> @this,
		Func<Failure, Task<TReturn>> fFail,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Ok<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			Failure f =>
				await fFail(f)
		};
}
