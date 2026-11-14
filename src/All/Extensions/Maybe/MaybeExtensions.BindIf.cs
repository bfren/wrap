// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="f"/> with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="f">Function that converts the value to a <see cref="Maybe{T}"/>, run when <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>The <see cref="Maybe{T}"/> returned by <paramref name="f"/>, or <see cref="None"/> if <paramref name="this"/> is <see cref="None"/> or <paramref name="fTest"/> returns <see langword="false"/>.</returns>
	public static Maybe<TReturn> BindIf<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> f) =>
		If(@this, fTest, f, _ => M.None);

	/// <inheritdoc cref="BindIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindIfAsync<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> f) =>
		IfAsync(@this, fTest, f, _ => M.None);

	/// <inheritdoc cref="BindIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindIfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> f) =>
		IfAsync(@this, fTest, f, _ => M.None);

	/// <inheritdoc cref="BindIf{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}})"/>
	public static Task<Maybe<TReturn>> BindIfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> f) =>
		IfAsync(@this, fTest, f, _ => M.None);
}
