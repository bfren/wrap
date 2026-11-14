// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>The <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Failure AssertFailure<T>(this Result<T> @this) =>
		Assert.IsType<Failure>(@this.Value); // the union's boxed case: @this itself boxes as Result<T>

	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Failure"/> with the expected message.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">[Optional] Expected arguments - not checked when empty.</param>
	/// <returns>The <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Failure AssertFailure<T>(this Result<T> @this, string message, params object?[] args) =>
		AssertFailure(@this).AssertMessage(message, args);

	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Failure"/> holding the expected exception.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="ex">Expected exception.</param>
	/// <returns>The <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Failure AssertFailure<T>(this Result<T> @this, Exception ex) =>
		AssertFailure(@this).AssertException(ex);

	/// <summary>
	/// Assert that <paramref name="this"/> is a <see cref="Failure"/> holding the expected exception and message.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="ex">Expected exception.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">[Optional] Expected arguments - not checked when empty.</param>
	/// <returns>The <see cref="Failure"/> held by <paramref name="this"/>.</returns>
	public static Failure AssertFailure<T>(this Result<T> @this, Exception ex, string message, params object?[] args) =>
		AssertFailure(@this, message, args).AssertException(ex);
}
