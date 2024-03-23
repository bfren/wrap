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
			F.LogException(failure.Exception);
		}
		else
		{
			F.LogFailure(failure.Message);
		}

		return new() { Value = failure };
	}

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> from a <see cref="FailValue" />.
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

	/// <inheritdoc cref="Fail{T}(string)"/>
	public static Fail Fail(string message)
	{
		F.LogFailure(message);

		return new() { Value = message };
	}

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> from a simple failure message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogFailure"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="message">Failure message.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(string message) =>
		Fail(message);

	/// <inheritdoc cref="Fail{T}(Exception)"/>
	public static Fail Fail(Exception ex)
	{
		F.LogException(ex);
		return new() { Value = ex };
	}

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> object from an exception type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure result.</returns>
	public static Fail Fail<TException>()
		where TException : Exception, new() =>
		Fail(new TException());

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> object from an exception.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="ex">Exception object.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(Exception ex) =>
		Fail(ex);
}
