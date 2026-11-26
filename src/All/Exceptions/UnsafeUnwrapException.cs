// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// The exception thrown by <c>Unsafe().Unwrap()</c> when the object holds a left (error / invalid) value,
/// for example a <c>Failure</c> or <c>None</c>.
/// </summary>
public sealed class UnsafeUnwrapException : WrapException
{
	/// <summary>
	/// Create an UnsafeUnwrapException for a left value that carries no exception.
	/// </summary>
	/// <remarks>
	/// The message includes <paramref name="left"/> converted to a string, and <see cref="Exception.InnerException"/>
	/// is <see langword="null"/>.
	/// </remarks>
	/// <param name="left">The left value that was found instead of a right value.</param>
	public UnsafeUnwrapException(object? left) : base(GetMessage(left))
	{ }

	/// <summary>
	/// Create an UnsafeUnwrapException for a left value that carries an exception.
	/// </summary>
	/// <remarks>
	/// The message includes <paramref name="left"/> converted to a string. <paramref name="inner"/> becomes
	/// <see cref="Exception.InnerException"/>, so the original stack trace is kept.
	/// </remarks>
	/// <param name="left">The left value that was found instead of a right value.</param>
	/// <param name="inner">The exception carried by <paramref name="left"/>, from <see cref="IWithException.Exception"/>.</param>
	public UnsafeUnwrapException(object? left, Exception inner) : base(GetMessage(left), inner)
	{ }

	private static string GetMessage(object? left) =>
		$"Unsafe Unwrap threw an exception: {left}";
}
