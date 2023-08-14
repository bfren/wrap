// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static T Unwrap<T>(this Maybe<T> @this, Func<T> ifNone) =>
		M.Switch(@this,
			none: ifNone,
			some: x => x
		);

	public static Task<T> UnwrapAsync<T>(this Maybe<T> @this, Func<Task<T>> ifNone) =>
		M.SwitchAsync(@this,
			none: ifNone,
			some: x => x
		);

	public static Task<T> UnwrapAsync<T>(this Task<Maybe<T>> @this, Func<T> ifNone) =>
		M.SwitchAsync(@this,
			none: ifNone,
			some: x => Task.FromResult(x)
		);

	public static Task<T> UnwrapAsync<T>(this Task<Maybe<T>> @this, Func<Task<T>> ifNone) =>
		M.SwitchAsync(@this,
			none: ifNone,
			some: x => Task.FromResult(x)
		);
}
