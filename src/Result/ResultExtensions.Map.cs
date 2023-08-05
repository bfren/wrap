// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class ResultExtensions
{
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> map) =>
		R.Switch(@this,
			err: x => R.Err(x),
			ok: x => R.Ok(map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> map) =>
		R.SwitchAsync(@this,
			err: x => R.Err(x),
			ok: async x => R.Ok(await map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> map) =>
		R.SwitchAsync(@this,
			err: x => R.Err(x),
			ok: x => R.Ok(map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> map) =>
		R.SwitchAsync(@this,
			err: x => R.Err(x),
			ok: async x => R.Ok(await map(x))
		);
}
