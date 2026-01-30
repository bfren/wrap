// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	#region Explicit Result<T>

	/// <summary>
	/// Create a failure result from a pre-existing <see cref="FailureValue"/>.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <param name="value">FailureValue.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(FailureValue value) =>
		new Failure(value);

	/// <summary>
	/// Create a failure result from a simple failure message.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(string message, params object?[] args) =>
		new Failure(message, args);

	/// <summary>
	/// Create a failure result from an exception type.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T, TException>()
		where TException : Exception, new() =>
		Fail<T>(new TException());

	/// <summary>
	/// Create a failure result from an exception.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <param name="ex">Exception.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(Exception ex) =>
		new Failure(ex);

	#endregion

	#region Implicit Result<T>

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from a pre-existing failure message.
	/// </summary>
	/// <param name="value">FailureValue.</param>
	/// <returns>FluentFail.</returns>
	public static Failure Fail(FailureValue value) =>
		new(value);

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from a simple failure message.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>FluentFail.</returns>
	public static Failure Fail(string message, params object?[] args) =>
		new(message, args);

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from an exception type.
	/// </summary>
	/// <typeparam name="TException">Exception type.</typeparam>
	/// <returns>FluentFail.</returns>
	public static Failure Fail<TException>()
		where TException : Exception, new() =>
		Fail(new TException());

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from an exception.
	/// </summary>
	/// <param name="ex">Exception.</param>
	/// <returns>FluentFail.</returns>
	public static Failure Fail<TException>(TException ex)
		where TException : Exception =>
		new(ex);

	#endregion
}
