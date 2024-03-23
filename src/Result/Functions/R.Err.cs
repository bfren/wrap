// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	/// <inheritdoc cref="Err{T}(FailValue)"/>
	public static Fail Err(FailValue error)
	{
		if (error.Exception is not null)
		{
			F.LogException(error.Exception);
		}
		else
		{
			F.LogError(error.Message);
		}

		return new() { Value = error };
	}

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> from an error value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the error using <see cref="F.LogException"/> or <see cref="F.LogError"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="error">Error value.</param>
	/// <returns>Error result.</returns>
	public static Result<T> Err<T>(FailValue error) =>
		Err(error);

	/// <inheritdoc cref="Err{T}(string)"/>
	public static Fail Err(string message)
	{
		F.LogError(message);

		return new() { Value = message };
	}

	/// <summary>
	/// Create an <see cref="Wrap.Fail"/> from a simple error message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the error using <see cref="F.LogError"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="message">Error message.</param>
	/// <returns>Error result.</returns>
	public static Result<T> Err<T>(string message) =>
		Err(message);

	/// <inheritdoc cref="Err{T}(Exception)"/>
	public static Fail Err(Exception ex)
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
	/// <returns>Error result.</returns>
	public static Fail Err<TException>()
		where TException : Exception, new() =>
		Err(new TException());

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
	/// <returns>Error result.</returns>
	public static Result<T> Err<T>(Exception ex) =>
		Err(ex);
}
