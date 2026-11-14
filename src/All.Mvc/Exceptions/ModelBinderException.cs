// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Mvc;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when a Wrap MVC model binder cannot be created - see
/// <see cref="MaybeModelBinderProvider.GetBinder(ModelBinderProviderContext)"/> and
/// <see cref="MonadModelBinderProvider.GetBinder(ModelBinderProviderContext)"/>.
/// </summary>
public sealed class ModelBinderException : Exception
{
	/// <summary>
	/// Create the exception with the default message.
	/// </summary>
	public ModelBinderException() { }

	/// <summary>
	/// Create the exception with a message.
	/// </summary>
	/// <param name="message">Exception message.</param>
	public ModelBinderException(string message) : base(message) { }

	/// <summary>
	/// Create the exception with a message and the exception that caused it.
	/// </summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Exception that caused this one.</param>
	public ModelBinderException(string message, Exception inner) : base(message, inner) { }
}
