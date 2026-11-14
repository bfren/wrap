// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Run <paramref name="fNone"/>, <paramref name="fFalse"/> or <paramref name="fTrue"/>, depending on the value of
	/// <paramref name="this"/> and the result of <paramref name="fTest"/>, and return its value.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	/// <param name="fNone">Run when <paramref name="this"/> is <see cref="None"/>.</param>
	/// <param name="fTest">Test to run on the value.</param>
	/// <param name="fFalse">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="false"/>.</param>
	/// <param name="fTrue">Run with the value when <paramref name="this"/> is <see cref="Some{T}"/> and <paramref name="fTest"/> returns <see langword="true"/>.</param>
	/// <returns>Value returned by <paramref name="fNone"/>, <paramref name="fFalse"/> or <paramref name="fTrue"/>.</returns>
	public static TReturn MatchIf<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<TReturn> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Maybe<T> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		@this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, bool> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
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
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, TReturn> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, TReturn> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				await fNone()
		};

	/// <inheritdoc cref="MatchIf{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, bool}, Func{T, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchIfAsync<T, TReturn>(this Task<Maybe<T>> @this,
		Func<Task<TReturn>> fNone,
		Func<T, Task<bool>> fTest,
		Func<T, Task<TReturn>> fFalse,
		Func<T, Task<TReturn>> fTrue
	) =>
		await @this switch
		{
			Some<T> x =>
				await fTest(x.Value) switch
				{
					false =>
						await fFalse(x.Value),

					true =>
						await fTrue(x.Value)
				},

			None =>
				await fNone()
		};
}
