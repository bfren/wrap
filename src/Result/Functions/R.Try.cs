// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class R
{
	/// <inheritdoc cref="Try{T}(Func{T}, ExceptionHandler)"/>
	public static Result<T> Try<T>(Func<T> f) =>
		Try(f, DefaultHandler);

	/// <summary>
	/// Execute function <paramref name="f"/>, returning its value and catching any exceptions.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="f">Function to run.</param>
	/// <param name="handler">Exception handler.</param>
	/// <returns>The value of <paramref name="f"/> or an <see cref="Wrap.Fail"/> result.</returns>
	public static Result<T> Try<T>(Func<T> f, ExceptionHandler handler)
	{
		try
		{
			return f();
		}
		catch (Exception ex)
		{
			return handler(ex);
		}
	}

	/// <inheritdoc cref="Try{T}(Func{T}, ExceptionHandler)"/>
	public static Task<Result<T>> TryAsync<T>(Func<Task<T>> f) =>
		TryAsync(f, DefaultHandler);

	/// <inheritdoc cref="Try{T}(Func{T}, ExceptionHandler)"/>
	public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> f, ExceptionHandler handler)
	{
		try
		{
			return await f();
		}
		catch (Exception ex)
		{
			return handler(ex);
		}
	}
}
