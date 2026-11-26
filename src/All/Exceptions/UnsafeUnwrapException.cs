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
	/// <inheritdoc cref="UnsafeUnwrapException(object?, Exception)"/>
	public UnsafeUnwrapException(object? left) : base(GetMessage(left))
	{ }

	/// <summary>
	/// Create an UnsafeUnwrapException.
	/// </summary>
	/// <remarks>
	/// The message includes <paramref name="left"/> converted to a string. If the left value implements
	/// <see cref="IWithException"/>, its exception becomes <see cref="Exception.InnerException"/>, so the original
	/// stack trace is kept.
	/// </remarks>
	/// <param name="left">The left value that was found instead of a right value.</param>
	/// <param name="inner">The exception carried by <paramref name="left"/>, or <see langword="null"/> if there is none.</param>
	public UnsafeUnwrapException(object? left, Exception inner) : base(GetMessage(left), inner)
	{ }

	private static string GetMessage(object? left) =>
		$"Unsafe Unwrap threw an exception: {left}";
}
