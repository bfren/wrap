// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	#region Return Value on Failure

	/// <summary>
	/// Return the value if <paramref name="this"/> is <see cref="Ok{T}"/>, otherwise the value returned by <paramref name="fFail"/>.
	/// </summary>
	/// <seealso cref="Result{T}.Unwrap(Func{Failure, T})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Returns the value to use, given the <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="fFail"/>.</returns>
	public static Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task<T>> fFail) =>
		R.MatchAsync(@this,
			fFail: fFail,
			fOk: x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, T> fFail) =>
		R.MatchAsync(@this,
			fFail: fFail,
			fOk: async x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task<T>> fFail) =>
		R.MatchAsync(@this,
			fFail: fFail,
			fOk: async x => x
		);

	#endregion

	#region Handle Failure then Return Value

	/// <summary>
	/// Return the value if <paramref name="this"/> is <see cref="Ok{T}"/>, otherwise run <paramref name="fFail"/>
	/// and return the value returned by <paramref name="fValue"/>.
	/// </summary>
	/// <seealso cref="Result{T}.Unwrap(Action{Failure}, Func{T})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fFail">Run with the <see cref="Failure"/>, for example to log it.</param>
	/// <param name="fValue">Returns the value to use if <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="fValue"/>.</returns>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task> fFail, Func<Task<T>> fValue)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await fFail(f);
				return await fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{Task{T}})"/>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task> fFail, Func<T> fValue)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await fFail(f);
				return fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{Task{T}})"/>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Action<Failure> fFail, Func<Task<T>> fValue)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				fFail(f);
				return await fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Action<Failure> fFail, Func<T> fValue)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				fFail(f);
				return fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> fFail, Func<T> fValue)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await fFail(f);
				return fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Action<Failure> fFail, Func<Task<T>> fValue)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				fFail(f);
				return await fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await fValue();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> fFail, Func<Task<T>> fValue)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await fFail(f);
				return await fValue();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await fValue();
		}
	}

	#endregion
}
