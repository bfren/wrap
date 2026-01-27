// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Create a failure result from a pre-existing <see cref="FailureValue"/>.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <param name="value">FailValue.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(FailureValue value) =>
		Result<T>.Failure.Create(value);

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from a pre-existing failure message.
	/// </summary>
	/// <param name="value">FailValue.</param>
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
	/// Start fluently creating a <see cref="Failure"/> from an exception.
	/// </summary>
	/// <typeparam name="T">Exception type.</typeparam>
	/// <returns>FluentFail.</returns>
	public static Failure Fail<T>()
		where T : Exception, new() =>
		Fail(new T());

	/// <summary>
	/// Start fluently creating a <see cref="Failure"/> from an exception.
	/// </summary>
	/// <param name="ex">Exception.</param>
	/// <returns>FluentFail.</returns>
	public static Failure Fail<T>(T ex)
		where T : Exception, new() =>
		new(ex);
}
