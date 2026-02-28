// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static void Match<T>(this Maybe<T> @this, Action fNone, Action<T> fSome) =>
		M.Match(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Action fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> fNone, Action<T> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Maybe<T> @this, Func<Task> fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T}(Maybe{T}, Action, Action{T})"/>
	public static Task MatchAsync<T>(this Task<Maybe<T>> @this, Func<Task> fNone, Func<T, Task> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static TReturn Match<T, TReturn>(this Maybe<T> @this, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		M.Match(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Maybe<T> @this, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		M.MatchAsync(@this, fNone, fSome);

	/// <inheritdoc cref="M.Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static Task<TReturn> MatchAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		M.MatchAsync(@this, fNone, fSome);
}
