// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

public static partial class ResultExtensions
{
	public static Result<TReturn> Map<T, TReturn>(this Result<T> @this, Func<T, TReturn> map) =>
		R.Match(@this,
			err: R.Err<TReturn>,
			ok: x => R.Try(() => map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Result<T> @this, Func<T, Task<TReturn>> map) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: x => R.TryAsync(() => map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, TReturn> map) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: x => R.Try(() => map(x))
		);

	public static Task<Result<TReturn>> MapAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<TReturn>> map) =>
		R.MatchAsync(@this,
			err: R.Err<TReturn>,
			ok: x => R.TryAsync(() => map(x))
		);
}
