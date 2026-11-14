// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="fTrue"/> or <paramref name="fFalse"/> with the value, depending on the result of <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fTrue">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <param name="fFalse">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <returns>The <see cref="Maybe{T}"/> returned by <paramref name="fTrue"/> or <paramref name="fFalse"/>, or <see cref="None"/> if <paramref name="this"/> is <see cref="None"/>.</returns>
	public static Maybe<TReturn> If<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> fTrue, Func<T, Maybe<TReturn>> fFalse) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> fTrue, Func<T, Task<Maybe<TReturn>>> fFalse) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> fTrue, Func<T, Maybe<TReturn>> fFalse) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> fTrue, Func<T, Task<Maybe<TReturn>>> fFalse) =>
		@this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> fTrue, Func<T, Maybe<TReturn>> fFalse) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> fTrue, Func<T, Maybe<TReturn>> fFalse) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Maybe<TReturn>> fTrue, Func<T, Task<Maybe<TReturn>>> fFalse) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				M.None
		};

	/// <inheritdoc cref="If{T, TReturn}(Maybe{T}, Func{T, bool}, Func{T, Maybe{TReturn}}, Func{T, Maybe{TReturn}})"/>
	public static async Task<Maybe<TReturn>> IfAsync<T, TReturn>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<Maybe<TReturn>>> fTrue, Func<T, Task<Maybe<TReturn>>> fFalse) =>
		await @this switch
		{
			Some<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				M.None
		};

	#region Special Case - Act if True

	/// <summary>
	/// Run <paramref name="fThen"/> with the value when <paramref name="fTest"/> returns <see langword="true"/>, otherwise keep the value.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fThen">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>The <see cref="Maybe{T}"/> returned by <paramref name="fThen"/>, or <paramref name="this"/> unchanged if <paramref name="fTest"/> returns <see langword="false"/> or <paramref name="this"/> is <see cref="None"/>.</returns>
	public static Maybe<T> If<T>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Maybe<T>> fThen) =>
		If(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfAsync<T>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<Maybe<T>>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Maybe<T>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<Maybe<T>>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	#endregion

	#region Special Case - Act if False

	/// <summary>
	/// Run <paramref name="fThen"/> with the value when <paramref name="fTest"/> returns <see langword="false"/>, otherwise keep the value.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fThen">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <returns>The <see cref="Maybe{T}"/> returned by <paramref name="fThen"/>, or <paramref name="this"/> unchanged if <paramref name="fTest"/> returns <see langword="true"/> or <paramref name="this"/> is <see cref="None"/>.</returns>
	public static Maybe<T> IfNot<T>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Maybe<T>> fThen) =>
		If(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfNotAsync<T>(this Maybe<T> @this, Func<T, bool> fTest, Func<T, Task<Maybe<T>>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfNotAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Maybe<T>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Maybe{T}, Func{T, bool}, Func{T, Maybe{T}})"/>
	public static Task<Maybe<T>> IfNotAsync<T>(this Task<Maybe<T>> @this, Func<T, bool> fTest, Func<T, Task<Maybe<T>>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	#endregion
}
