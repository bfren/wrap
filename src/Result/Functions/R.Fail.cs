// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	#region Explicit Result<T>

	/// <summary>
	/// Return an existing <see cref="Failure"/> as a failure result.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="value">Failure.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(Failure value) =>
		value;

	/// <summary>
	/// Create a failure result from a message, with the <see cref="Logging.LogLevel.Warning"/> log level.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(string message, params object?[] args) =>
		new Failure(message, args);

	/// <summary>
	/// Create a failure result from a new exception of type <typeparamref name="TException"/>, with the <see cref="Logging.LogLevel.Error"/> log level.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T, TException>()
		where TException : Exception, new() =>
		Fail<T>(new TException());

	/// <summary>
	/// Create a failure result from an exception, using its message and the <see cref="Logging.LogLevel.Error"/> log level.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="ex">Exception.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(Exception ex) =>
		new Failure(ex);

	#endregion

	#region Implicit Result<T>

	/// <summary>
	/// Return an existing <see cref="Failure"/> unchanged.
	/// </summary>
	/// <param name="value">Failure.</param>
	/// <returns><paramref name="value"/>, which converts to any <see cref="Result{T}"/>.</returns>
	public static Failure Fail(Failure value) =>
		value;

	/// <summary>
	/// Create a <see cref="Failure"/> from a message, with the <see cref="Logging.LogLevel.Warning"/> log level.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure, which converts to any <see cref="Result{T}"/>.</returns>
	public static Failure Fail(string message, params object?[] args) =>
		new(message, args);

	/// <summary>
	/// Create a <see cref="Failure"/> from a new exception of type <typeparamref name="TException"/>, with the <see cref="Logging.LogLevel.Error"/> log level.
	/// </summary>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure, which converts to any <see cref="Result{T}"/>.</returns>
	public static Failure Fail<TException>()
		where TException : Exception, new() =>
		Fail(new TException());

	/// <summary>
	/// Create a <see cref="Failure"/> from an exception, using its message and the <see cref="Logging.LogLevel.Error"/> log level.
	/// </summary>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <param name="ex">Exception.</param>
	/// <returns>Failure, which converts to any <see cref="Result{T}"/>.</returns>
	public static Failure Fail<TException>(TException ex)
		where TException : Exception =>
		new(ex);

	#endregion
}
