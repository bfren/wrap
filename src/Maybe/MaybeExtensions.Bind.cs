// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static Maybe<TReturn> Bind<T, TReturn>(this Maybe<T> @this, Func<T, Maybe<TReturn>> bind) =>
		M.Switch(@this,
			none: M.None,
			some: bind
		);

	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<Maybe<TReturn>>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);

	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Maybe<TReturn>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);

	public static Task<Maybe<TReturn>> BindAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<Maybe<TReturn>>> bind) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: bind
		);
}
