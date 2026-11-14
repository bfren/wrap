// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Run <paramref name="fTrue"/> or <paramref name="fFalse"/> with the value, depending on the result of <paramref name="fTest"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fTrue">Run with the value when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <param name="fFalse">Run with the value when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <returns>The <see cref="Result{T}"/> returned by <paramref name="fTrue"/> or <paramref name="fFalse"/>, or the <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Result<TReturn> If<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> fTrue, Func<T, Result<TReturn>> fFalse) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> fTrue, Func<T, Result<TReturn>> fFalse) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> fTrue, Func<T, Task<Result<TReturn>>> fFalse) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> fTrue, Func<T, Task<Result<TReturn>>> fFalse) =>
		@this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> fTrue, Func<T, Result<TReturn>> fFalse) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> fTrue, Func<T, Result<TReturn>> fFalse) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Result<TReturn>> fTrue, Func<T, Task<Result<TReturn>>> fFalse) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	/// <inheritdoc cref="If{T, TReturn}(Result{T}, Func{T, bool}, Func{T, Result{TReturn}}, Func{T, Result{TReturn}})"/>
	public static async Task<Result<TReturn>> IfAsync<T, TReturn>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<Result<TReturn>>> fTrue, Func<T, Task<Result<TReturn>>> fFalse) =>
		await @this switch
		{
			Ok<T> x =>
				fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			{ } x =>
				x.PassFailure<TReturn>()
		};

	#region Special Case - Act if True

	/// <summary>
	/// Run <paramref name="fThen"/> with the value when <paramref name="fTest"/> returns <see langword="true"/>, otherwise keep the value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fThen">Run with the value when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>The <see cref="Result{T}"/> returned by <paramref name="fThen"/>, or <paramref name="this"/> unchanged if <paramref name="fTest"/> returns <see langword="false"/> or <paramref name="this"/> is a <see cref="Failure"/>.</returns>
	public static Result<T> If<T>(this Result<T> @this, Func<T, bool> fTest, Func<T, Result<T>> fThen) =>
		If(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfAsync<T>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<Result<T>>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Result<T>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	/// <inheritdoc cref="If{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<Result<T>>> fThen) =>
		IfAsync(@this, fTest, fThen, x => x);

	#endregion

	#region Special Case - Act if False

	/// <summary>
	/// Run <paramref name="fThen"/> with the value when <paramref name="fTest"/> returns <see langword="false"/>, otherwise keep the value.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fThen">Run with the value when <paramref name="this"/> is <see cref="Ok{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <returns>The <see cref="Result{T}"/> returned by <paramref name="fThen"/>, or <paramref name="this"/> unchanged if <paramref name="fTest"/> returns <see langword="true"/> or <paramref name="this"/> is a <see cref="Failure"/>.</returns>
	public static Result<T> IfNot<T>(this Result<T> @this, Func<T, bool> fTest, Func<T, Result<T>> fThen) =>
		If(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfNotAsync<T>(this Result<T> @this, Func<T, bool> fTest, Func<T, Task<Result<T>>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfNotAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Result<T>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	/// <inheritdoc cref="IfNot{T}(Result{T}, Func{T, bool}, Func{T, Result{T}})"/>
	public static Task<Result<T>> IfNotAsync<T>(this Task<Result<T>> @this, Func<T, bool> fTest, Func<T, Task<Result<T>>> fThen) =>
		IfAsync(@this, fTest, x => x, fThen);

	#endregion
}
