// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class ResultExtensions
{
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> map) =>
		R.Switch(@this,
			err: R.Err<TReturn>,
			ok: x => R.Wrap(map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> map) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: async x => R.Wrap(await map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> map) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: x => R.Wrap(map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> map) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: async x => R.Wrap(await map(x))
		);
}
