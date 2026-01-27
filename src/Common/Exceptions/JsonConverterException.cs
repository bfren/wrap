// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Throw when <see cref="Json.UnionJsonConverterFactory"/> is unable to create a JsonConverter.
/// </summary>
/// <param name="message">Exception message.</param>
public sealed class JsonConverterException(string message) :
	WrapException(message);
