// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="Json.MonadJsonConverterFactory"/> cannot create a JSON converter for a type.
/// </summary>
/// <param name="message">Exception message.</param>
public sealed class JsonConverterException(string message) :
	WrapException(message);
