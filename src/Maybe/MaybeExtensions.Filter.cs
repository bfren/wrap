// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static Maybe<T> Filter<T>(this Maybe<T> @this, Func<T, bool> predicate) =>
		@this.Bind(x => predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	public static Task<Maybe<T>> FilterAsync<T>(this Maybe<T> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> predicate) =>
		@this.BindAsync(x => predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});

	public static Task<Maybe<T>> FilterAsync<T>(this Task<Maybe<T>> @this, Func<T, Task<bool>> predicate) =>
		@this.BindAsync(async x => await predicate(x) switch
		{
			false =>
				M.None,

			true =>
				M.Wrap(x)
		});
}
