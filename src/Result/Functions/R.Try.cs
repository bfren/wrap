// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monads;

public static partial class R
{
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
