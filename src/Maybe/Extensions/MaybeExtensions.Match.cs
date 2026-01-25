// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static void Match<T>(this Maybe<T> @this, Action none, Action<T> some) =>
		M.Match(@this, none, some);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Action none, Func<T, Task> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> none, Action<T> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> none, Func<T, Task> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Maybe<T>> @this, Func<Task> none, Func<T, Task> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static TReturn Match<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, TReturn> some) =>
		M.Match(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static TReturn Match<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.Match(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, TReturn> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, TReturn none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, TReturn> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, TReturn> some) =>
		M.MatchAsync(@this, none, some);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> none, Func<T, Task<TReturn>> some) =>
		M.MatchAsync(@this, none, some);
}
