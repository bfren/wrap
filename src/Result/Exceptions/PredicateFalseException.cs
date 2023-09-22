// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Exceptions;

/// <summary>
/// Not really an 'exception' but used so <see cref="ResultExtensions.Filter{T}(Result{T}, Func{T, bool})"/>
/// can return an <see cref="Err"/> object when a predicate fails.
/// </summary>
/// <seealso cref="ResultExtensions.Filter{T}(Result{T}, Func{T, bool})"/>
public sealed class PredicateFalseException : Exception
{
	/// <summary>Create exception.</summary>
	public PredicateFalseException() { }
}
