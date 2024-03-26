// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	/// <inheritdoc cref="Fail{T}(FailValue)"/>
	public static Fail Fail(FailValue failure)
	{
		if (failure.Exception is not null)
		{
			F.LogException?.Invoke(failure.Exception);
		}
		else
		{
			F.LogFailure?.Invoke(failure.Message, [.. failure.Args]);
		}

		return new() { Value = failure };
	}

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> from a <see cref="FailValue" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the failure using <see cref="F.LogException"/> or <see cref="F.LogFailure"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="failure">Failure value.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(FailValue failure) =>
		Fail(failure);

	#region Without Context

	/// <inheritdoc cref="Fail(string, string, string, object[])"/>
	public static Fail Fail(string message, params object[] args) =>
		Fail(FailValue.Create(message, args));

	/// <inheritdoc cref="Fail{TContext, TException}()"/>
	public static Fail Fail<TException>()
		where TException : Exception, new() =>
		Fail(FailValue.Create(new TException()));

	/// <inheritdoc cref="Fail{T}(Exception)"/>
	public static Fail Fail(Exception ex) =>
		Fail(FailValue.Create(ex));

	#endregion

	#region With Context

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> from a simple failure message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the failure using <see cref="F.LogException"/> or <see cref="F.LogFailure"/> first.
	/// </para>
	/// </remarks>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure result.</returns>
	public static Fail Fail(string @class, string function, string message, params object[] args) =>
		Fail(FailValue.Create(@class, function, message, args));

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> from a simple failure message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the failure using <see cref="F.LogException"/> or <see cref="F.LogFailure"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Failure context.</typeparam>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure result.</returns>
	public static Fail Fail<T>(string message, params object[] args) =>
		Fail(FailValue.Create<T>(message, args));

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> object from an exception type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <param name="class">Context class.</param>
	/// <param name="function">Context function.</param>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure result.</returns>
	public static Fail Fail<TException>(string @class, string function)
		where TException : Exception, new() =>
		Fail(FailValue.Create(@class, function, new TException()));

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> object from an exception type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="TContext">Failure context.</typeparam>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure result.</returns>
	public static Fail Fail<TContext, TException>()
		where TException : Exception, new() =>
		Fail(FailValue.Create<TContext>(new TException()));

	/// <summary>
	/// Create a <see cref="Wrap.Fail"/> object from an exception.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Failure context.</typeparam>
	/// <param name="ex">Exception object.</param>
	/// <returns>Failure result.</returns>
	public static Fail Fail<T>(Exception ex) =>
		Fail(FailValue.Create<T>(ex));

	#endregion
}
