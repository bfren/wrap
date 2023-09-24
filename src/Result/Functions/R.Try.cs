// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Execute function <paramref name="f"/>, returning its value and catching any exceptions.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="f">Function to run.</param>
	/// <returns>The value of <paramref name="f"/> or an <see cref="Wrap.Err"/> result.</returns>
	public static Result<T> Try<T>(Func<T> f)
	{
		try
		{
			return f();
		}
		catch (Exception ex)
		{
			return Err(ex);
		}
	}

	/// <inheritdoc cref="Try{T}(Func{T})"/>
	public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> f)
	{
		try
		{
			return await f();
		}
		catch (Exception ex)
		{
			return Err(ex);
		}
	}
}
