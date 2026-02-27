// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap;

public static partial class M
{
	#region Without Return Value

	/// <summary>
	/// Run an action based on the value of <paramref name="maybe"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It would be possible to achieve the same thing using switch statements, but doing that would add the
	/// potential for not handling both cases (<see cref="Some{T}"/> and <see cref="Wrap.None"/>).
	/// </para>
	/// <para>
	/// This way there has to be a function to handle both cases.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="maybe">Maybe object.</param>
	/// <param name="fNone">Function to run when <paramref name="maybe"/> is <see cref="Wrap.None"/>.</param>
	/// <param name="fSome">Function to run when <paramref name="maybe"/> is <see cref="Some{T}"/>.</param>
	/// <exception cref="InvalidMaybeTypeException"></exception>
	/// <exception cref="NullMaybeException"></exception>
	public static void Match<T>(Maybe<T> maybe, Action fNone, Action<T> fSome)
	{
		switch (maybe)
		{
			case Maybe<T>.NoneImpl:
				fNone();
				return;

			case Some<T> x:
				fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Maybe<T> maybe, Action fNone, Func<T, Task> fSome)
	{
		switch (maybe)
		{
			case Maybe<T>.NoneImpl:
				fNone();
				return;

			case Some<T> x:
				await fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Maybe<T> maybe, Func<Task> fNone, Action<T> fSome)
	{
		switch (maybe)
		{
			case Maybe<T>.NoneImpl:
				await fNone();
				return;

			case Some<T> x:
				fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Maybe<T> maybe, Func<Task> fNone, Func<T, Task> fSome)
	{
		switch (maybe)
		{
			case Maybe<T>.NoneImpl:
				await fNone();
				return;

			case Some<T> x:
				await fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Maybe<T>> maybe, Action fNone, Action<T> fSome)
	{
		switch (await maybe)
		{
			case Maybe<T>.NoneImpl:
				fNone();
				return;

			case Some<T> x:
				fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Maybe<T>> maybe, Action fNone, Func<T, Task> fSome)
	{
		switch (await maybe)
		{
			case Maybe<T>.NoneImpl:
				fNone();
				return;

			case Some<T> x:
				await fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Maybe<T>> maybe, Func<Task> fNone, Action<T> fSome)
	{
		switch (await maybe)
		{
			case Maybe<T>.NoneImpl:
				await fNone();
				return;

			case Some<T> x:
				fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	/// <inheritdoc cref="Match{T}(Maybe{T}, Action, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Maybe<T>> maybe, Func<Task> fNone, Func<T, Task> fSome)
	{
		switch (await maybe)
		{
			case Maybe<T>.NoneImpl:
				await fNone();
				return;

			case Some<T> x:
				await fSome(x.Value);
				return;

			case { } m:
				throw new InvalidMaybeTypeException(m.GetType());

			default:
				throw new NullMaybeException();
		}
	}

	#endregion

	#region With Return Value

	/// <summary>
	/// Run a function based on the value of <paramref name="maybe"/> and return its value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It would be possible to achieve the same thing using switch statements, but doing that would add the
	/// potential for not handling both cases (<see cref="Some{T}"/> and <see cref="Wrap.None"/>).
	/// </para>
	/// <para>
	/// This way there has to be a function to handle both cases.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="maybe">Maybe object.</param>
	/// <param name="fNone">Function to run when <paramref name="maybe"/> is <see cref="Wrap.None"/>.</param>
	/// <param name="fSome">Function to run when <paramref name="maybe"/> is <see cref="Some{T}"/>.</param>
	/// <returns>Value generated by either <paramref name="fNone"/> or <paramref name="fSome"/>.</returns>
	/// <exception cref="InvalidMaybeTypeException"></exception>
	/// <exception cref="NullMaybeException"></exception>
	public static TReturn Match<T, TReturn>(Maybe<T> maybe, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		maybe switch
		{
			Maybe<T>.NoneImpl =>
				fNone(),

			Some<T> x =>
				fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Maybe<T> maybe, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		maybe switch
		{
			Maybe<T>.NoneImpl =>
				fNone(),

			Some<T> x =>
				await fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Maybe<T> maybe, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		maybe switch
		{
			Maybe<T>.NoneImpl =>
				await fNone(),

			Some<T> x =>
				fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Maybe<T> maybe, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		maybe switch
		{
			Maybe<T>.NoneImpl =>
				await fNone(),

			Some<T> x =>
				await fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<TReturn> fNone, Func<T, TReturn> fSome) =>
		await maybe switch
		{
			Maybe<T>.NoneImpl =>
				fNone(),

			Some<T> x =>
				fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<TReturn> fNone, Func<T, Task<TReturn>> fSome) =>
		await maybe switch
		{
			Maybe<T>.NoneImpl =>
				fNone(),

			Some<T> x =>
				await fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<Task<TReturn>> fNone, Func<T, TReturn> fSome) =>
		await maybe switch
		{
			Maybe<T>.NoneImpl =>
				await fNone(),

			Some<T> x =>
				fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Maybe{T}, Func{TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Maybe<T>> maybe, Func<Task<TReturn>> fNone, Func<T, Task<TReturn>> fSome) =>
		await maybe switch
		{
			Maybe<T>.NoneImpl =>
				await fNone(),

			Some<T> x =>
				await fSome(x.Value),

			{ } m =>
				throw new InvalidMaybeTypeException(m.GetType()),

			_ =>
				throw new NullMaybeException()
		};

	#endregion
}
