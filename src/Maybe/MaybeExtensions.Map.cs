// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static Maybe<TReturn> Map<T, TReturn>(this Maybe<T> @this, Func<T, TReturn> map) =>
		M.Switch(@this,
			none: M.None,
			some: x => M.Wrap(map(x))
		);

	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Maybe<T> @this, Func<T, Task<TReturn>> map) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: async x => M.Wrap(await map(x))
		);

	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, TReturn> map) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: x => M.Wrap(map(x))
		);

	public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, Task<TReturn>> map) =>
		M.SwitchAsync(@this,
			none: M.None,
			some: async x => M.Wrap(await map(x))
		);
}
