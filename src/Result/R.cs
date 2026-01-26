// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Exceptions;

namespace Wrap;

/// <summary>
/// Pure functions for interacting with <see cref="Result{TOk}"/> objects.
/// </summary>
public static partial class R
{
	/// <summary>
	/// Allows custom error handling when an operation fails.
	/// </summary>
	/// <returns>FluentFailure object</returns>
	public delegate FluentFailure ErrorHandler();

	/// <summary>
	/// Allows custom error handling when an operation fails.
	/// </summary>
	/// <param name="message">Error message to bubble up.</param>
	/// <param name="args">[Optional] message arguments.</param>
	/// <returns>FluentFailure object</returns>
	public delegate FluentFailure ErrorHandlerWithMsg(string message, params object?[] args);

	/// <summary>
	/// Handles exceptions when an operation fails - see <see cref="Try{T}(Func{T})"/>.
	/// </summary>
	/// <param name="e">Exception object.</param>
	/// <returns>FluentFailure object.</returns>
	public delegate FluentFailure ExceptionHandler(Exception e);

	/// <summary>
	/// Default exception handler.
	/// </summary>
	public static ExceptionHandler DefaultHandler =>
		Fail;

	/// <summary>
	/// Throw a <see cref="FailureException"/> using <paramref name="failure"/>.
	/// </summary>
	/// <typeparam name="T">Return type.</typeparam>
	/// <param name="failure">Failure value.</param>
	/// <returns>Nothing - return type is specified so this can be used as a delegate.</returns>
	/// <exception cref="FailureException"></exception>
	public static T ThrowFailure<T>(FailureValue failure) =>
		throw new FailureException(failure);
}
