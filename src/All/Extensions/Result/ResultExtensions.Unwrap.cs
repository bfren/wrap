// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	#region Return Value on Failure

	/// <summary>
	/// Return the value if <paramref name="this"/> is <see cref="Ok{T}"/>, otherwise the value returned by <paramref name="ifFailed"/>.
	/// </summary>
	/// <seealso cref="Result{T}.Unwrap(Func{Failure, T})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="ifFailed">Returns the value to use, given the <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="ifFailed"/>.</returns>
	public static Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task<T>> ifFailed) =>
		R.MatchAsync(@this,
			fFail: ifFailed,
			fOk: x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, T> ifFailed) =>
		R.MatchAsync(@this,
			fFail: ifFailed,
			fOk: async x => x
		);

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task{T}})"/>
	public static Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task<T>> ifFailed) =>
		R.MatchAsync(@this,
			fFail: ifFailed,
			fOk: async x => x
		);

	#endregion

	#region Handle Failure then Return Value

	/// <summary>
	/// Return the value if <paramref name="this"/> is <see cref="Ok{T}"/>, otherwise run <paramref name="ifFailed"/>
	/// and return the value returned by <paramref name="returnThis"/>.
	/// </summary>
	/// <seealso cref="Result{T}.Unwrap(Action{Failure}, Func{T})"/>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="ifFailed">Run with the <see cref="Failure"/>, for example to log it.</param>
	/// <param name="returnThis">Returns the value to use if <paramref name="this"/> is a <see cref="Failure"/>.</param>
	/// <returns>The <see cref="Ok{T}"/> value, or the value returned by <paramref name="returnThis"/>.</returns>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task> ifFailed, Func<Task<T>> returnThis)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await ifFailed(f);
				return await returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{Task{T}})"/>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Func<Failure, Task> ifFailed, Func<T> returnThis)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await ifFailed(f);
				return returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{Task{T}})"/>
	public static async Task<T> UnwrapAsync<T>(this Result<T> @this, Action<Failure> ifFailed, Func<Task<T>> returnThis)
	{
		switch (@this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				ifFailed(f);
				return await returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Action<Failure> ifFailed, Func<T> returnThis)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				ifFailed(f);
				return returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> ifFailed, Func<T> returnThis)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await ifFailed(f);
				return returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Action<Failure> ifFailed, Func<Task<T>> returnThis)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				ifFailed(f);
				return await returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await returnThis();
		}
	}

	/// <inheritdoc cref="UnwrapAsync{T}(Result{T}, Func{Failure, Task}, Func{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Result<T>> @this, Func<Failure, Task> ifFailed, Func<Task<T>> returnThis)
	{
		switch (await @this)
		{
			case Ok<T> x:
				return x.Value;

			case Failure f:
				await ifFailed(f);
				return await returnThis();

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				return await returnThis();
		}
	}

	#endregion
}
