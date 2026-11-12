// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Run <paramref name="f"/>, returning its value and catching any exceptions with <see cref="DefaultExceptionHandler"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="f">Function to run.</param>
	/// <returns>
	/// The value of <paramref name="f"/> as an <see cref="Ok{T}"/> (or a <see cref="Failure"/> if it is <see langword="null"/>),
	/// or a <see cref="Failure"/> holding the exception if <paramref name="f"/> throws.
	/// </returns>
	public static Result<T> Try<T>(Func<T> f) =>
		Try(f, DefaultExceptionHandler);

	/// <summary>
	/// Run <paramref name="f"/>, returning its value and catching any exceptions with <paramref name="e"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="f">Function to run.</param>
	/// <param name="e">Exception handler.</param>
	/// <returns>
	/// The value of <paramref name="f"/> as an <see cref="Ok{T}"/> (or a <see cref="Failure"/> if it is <see langword="null"/>),
	/// or the <see cref="Failure"/> returned by <paramref name="e"/> if <paramref name="f"/> throws.
	/// </returns>
	public static Result<T> Try<T>(Func<T> f, ExceptionHandler e)
	{
		try
		{
			return f();
		}
		catch (Exception ex)
		{
			return e(ex);
		}
	}

	/// <inheritdoc cref="Try{T}(Func{T})"/>
	public static Task<Result<T>> TryAsync<T>(Func<Task<T>> f) =>
		TryAsync(f, DefaultExceptionHandler);

	/// <inheritdoc cref="Try{T}(Func{T}, ExceptionHandler)"/>
	public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> f, ExceptionHandler e)
	{
		try
		{
			return await f();
		}
		catch (Exception ex)
		{
			return e(ex);
		}
	}
}
