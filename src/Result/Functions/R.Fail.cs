// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Create a failure result from a pre-existing <see cref="FailValue"/>.
	/// </summary>
	/// <typeparam name="T">Ok result type.</typeparam>
	/// <param name="value">FailValue.</param>
	/// <returns>Failure result.</returns>
	public static Result<T> Fail<T>(FailValue value) =>
		Result<T>.Failure.Create(value);

	/// <summary>
	/// Start fluently creating a <see cref="Wrap.Fail"/> from a simple failure message.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>FluentFail.</returns>
	public static FluentFailure Fail(string message, object? args = null) =>
		new(message, args);

	/// <summary>
	/// Start fluently creating a <see cref="Wrap.Fail"/> from an exception.
	/// </summary>
	/// <typeparam name="T">Exception type.</typeparam>
	/// <returns>FluentFail.</returns>
	public static FluentFailure Fail<T>()
		where T : Exception, new() =>
		Fail(new T());

	/// <summary>
	/// Start fluently creating a <see cref="Wrap.Fail"/> from an exception.
	/// </summary>
	/// <param name="ex">Exception.</param>
	/// <returns>FluentFail.</returns>
	public static FluentFailure Fail<T>(T ex)
		where T : Exception, new() =>
		new(ex);
}
