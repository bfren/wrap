// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;

namespace Wrap;

/// <summary>
/// A value that may carry the exception that caused it, for example a <c>Failure</c>.
/// </summary>
/// <remarks>
/// <c>Unsafe().Unwrap()</c> checks for this interface on a left value, and when it finds one it passes
/// <see cref="Exception"/> on as the <see cref="Exception.InnerException"/> of the exception it throws,
/// so the original stack trace is kept.
/// </remarks>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
	Justification = "Names the Exception property it exposes, matching IWithId.")]
public interface IWithException
{
	/// <summary>
	/// Gets the exception that caused this value, or <see langword="null"/> if there was none.
	/// </summary>
	Exception? Exception { get; init; }
}
