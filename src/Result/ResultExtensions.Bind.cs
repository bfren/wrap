// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class ResultExtensions
{
	public static Result<TReturn> Bind<T, TReturn>(this Result<T> @this, Func<T, Result<TReturn>> bind) =>
		R.Switch(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Result<T> @this, Func<T, Task<Result<TReturn>>> bind) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Result<TReturn>> bind) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);

	public static Task<Result<TReturn>> BindAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, Task<Result<TReturn>>> bind) =>
		R.SwitchAsync(@this,
			err: R.Err<TReturn>,
			ok: bind
		);
}
