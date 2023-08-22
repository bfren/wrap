// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Monadic.Exceptions;

namespace Monadic;

public static partial class R
{
	/// <inheritdoc cref="Err{T}(Exception)"/>
	public static Err Err(Exception ex)
	{
		F.LogException(ex);
		return new(ex);
	}

	/// <summary>
	/// Create an <see cref="Monadic.Err"/> object from an exception type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Error result.</returns>
	public static Err Err<TException>()
		where TException : Exception, new() =>
		Err(new TException());

	/// <summary>
	/// Create an <see cref="Monadic.Err"/> object from an exception.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the exception using <see cref="F.LogException"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="ex">Exception object.</param>
	/// <returns>Error result.</returns>
	public static Result<T> Err<T>(Exception ex) =>
		Err(ex);

	/// <inheritdoc cref="Err{T}(string)"/>
	public static Err Err(string error)
	{
		F.LogError(error);
		return new(new SimpleErrorException(error));
	}

	/// <summary>
	/// Create an <see cref="Monadic.Err"/> from a simple error message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Also logs the error using <see cref="F.LogError"/> first.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="error">Error message.</param>
	/// <returns>Error result.</returns>
	public static Result<T> Err<T>(string error) =>
		Err(error);
}
