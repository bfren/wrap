// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Mvc;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when creating a StrongId MVC model binder fails -
/// see <see cref="MonadModelBinderProvider.GetBinder(ModelBinderProviderContext)"/>.
/// </summary>
public sealed class ModelBinderException : Exception
{
	/// <summary>
	/// Create object.
	/// </summary>
	public ModelBinderException() { }

	/// <summary>
	/// Create object with message.
	/// </summary>
	/// <param name="message">Exception message.</param>
	public ModelBinderException(string message) : base(message) { }

	/// <summary>
	/// Create object with message and inner exception.
	/// </summary>
	/// <param name="message">Exception message.</param>
	/// <param name="inner">Inner Exception.</param>
	public ModelBinderException(string message, Exception inner) : base(message, inner) { }
}
