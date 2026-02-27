// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap;

public static partial class R
{
	#region Without Return Value

	/// <summary>
	/// Run an action based on the value of <paramref name="result"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It would be possible to achieve the same thing using switch statements, but doing that would add the
	/// potential for not handling both cases (<see cref="Ok{T}"/> and <see cref="Failure"/>).
	/// </para>
	/// <para>
	/// This way there has to be a function to handle both cases.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="result">Result object.</param>
	/// <param name="fFail">Function to run when <paramref name="result"/> is <see cref="Failure"/>.</param>
	/// <param name="fOk">Function to run when <paramref name="result"/> is <see cref="Ok{T}"/>.</param>
	/// <exception cref="InvalidResultTypeException"></exception>
	/// <exception cref="NullResultException"></exception>
	public static void Match<T>(Result<T> result, Action<FailureValue> fFail, Action<T> fOk)
	{
		switch (result)
		{
			case Result<T>.FailureImpl f:
				fFail(f.Value);
				return;

			case Ok<T> x:
				fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Result<T> result, Action<FailureValue> fFail, Func<T, Task> fOk)
	{
		switch (result)
		{
			case Result<T>.FailureImpl f:
				fFail(f.Value);
				return;

			case Ok<T> x:
				await fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Result<T> result, Func<FailureValue, Task> fFail, Action<T> fOk)
	{
		switch (result)
		{
			case Result<T>.FailureImpl f:
				await fFail(f.Value);
				return;

			case Ok<T> x:
				fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Result<T> result, Func<FailureValue, Task> fFail, Func<T, Task> fOk)
	{
		switch (result)
		{
			case Result<T>.FailureImpl f:
				await fFail(f.Value);
				return;

			case Ok<T> x:
				await fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Result<T>> result, Action<FailureValue> fFail, Action<T> fOk)
	{
		switch (await result)
		{
			case Result<T>.FailureImpl f:
				fFail(f.Value);
				return;

			case Ok<T> x:
				fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Result<T>> result, Func<FailureValue, Task> fFail, Action<T> fOk)
	{
		switch (await result)
		{
			case Result<T>.FailureImpl f:
				await fFail(f.Value);
				return;

			case Ok<T> x:
				fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Result<T>> result, Action<FailureValue> fFail, Func<T, Task> fOk)
	{
		switch (await result)
		{
			case Result<T>.FailureImpl f:
				fFail(f.Value);
				return;

			case Ok<T> x:
				await fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	/// <inheritdoc cref="Match{T}(Result{T}, Action{FailureValue}, Action{T})"/>
	public static async Task MatchAsync<T>(Task<Result<T>> result, Func<FailureValue, Task> fFail, Func<T, Task> fOk)
	{
		switch (await result)
		{
			case Result<T>.FailureImpl f:
				await fFail(f.Value);
				return;

			case Ok<T> x:
				await fOk(x.Value);
				return;

			case { } m:
				throw new InvalidResultTypeException(m.GetType());

			default:
				throw new NullResultException();
		}
	}

	#endregion

	#region With Return Value

	/// <summary>
	/// Run a function based on the value of <paramref name="result"/> and return its value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It would be possible to achieve the same thing using switch statements, but doing that would add the
	/// potential for not handling both cases (<see cref="Ok{T}"/> and <see cref="Failure"/>).
	/// </para>
	/// <para>
	/// This way there has to be a function to handle both cases.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TReturn">Return value type.</typeparam>
	/// <param name="result">Result object.</param>
	/// <param name="fFail">Function to run when <paramref name="result"/> is <see cref="Failure"/>.</param>
	/// <param name="fOk">Function to run when <paramref name="result"/> is <see cref="Ok{T}"/>.</param>
	/// <returns>Value generated by either <paramref name="fFail"/> or <paramref name="fOk"/>.</returns>
	/// <exception cref="InvalidResultTypeException"></exception>
	/// <exception cref="NullResultException"></exception>
	public static TReturn Match<T, TReturn>(Result<T> result, Func<FailureValue, TReturn> fFail, Func<T, TReturn> fOk) =>
		result switch
		{
			Result<T>.FailureImpl x =>
				fFail(x.Value),

			Ok<T> y =>
				fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Result<T> result, Func<FailureValue, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		result switch
		{
			Result<T>.FailureImpl x =>
				fFail(x.Value),

			Ok<T> y =>
				await fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Result<T> result, Func<FailureValue, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		result switch
		{
			Result<T>.FailureImpl x =>
				await fFail(x.Value),

			Ok<T> y =>
				fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Result<T> result, Func<FailureValue, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		result switch
		{
			Result<T>.FailureImpl x =>
				await fFail(x.Value),

			Ok<T> y =>
				await fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Result<T>> result, Func<FailureValue, TReturn> fFail, Func<T, TReturn> fOk) =>
		await result switch
		{
			Result<T>.FailureImpl x =>
				fFail(x.Value),

			Ok<T> y =>
				fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Result<T>> result, Func<FailureValue, TReturn> fFail, Func<T, Task<TReturn>> fOk) =>
		await result switch
		{
			Result<T>.FailureImpl x =>
				fFail(x.Value),

			Ok<T> y =>
				await fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Result<T>> result, Func<FailureValue, Task<TReturn>> fFail, Func<T, TReturn> fOk) =>
		await result switch
		{
			Result<T>.FailureImpl x =>
				await fFail(x.Value),

			Ok<T> y =>
				fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	/// <inheritdoc cref="Match{T, TReturn}(Result{T}, Func{FailureValue, TReturn}, Func{T, TReturn})"/>
	public static async Task<TReturn> MatchAsync<T, TReturn>(Task<Result<T>> result, Func<FailureValue, Task<TReturn>> fFail, Func<T, Task<TReturn>> fOk) =>
		await result switch
		{
			Result<T>.FailureImpl x =>
				await fFail(x.Value),

			Ok<T> y =>
				await fOk(y.Value),

			{ } r =>
				throw new InvalidResultTypeException(r.GetType()),

			_ =>
				throw new NullResultException()
		};

	#endregion
}
