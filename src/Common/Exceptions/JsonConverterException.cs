// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Throw when <see cref="Json.UnionJsonConverterFactory"/> is unable to create a JsonConverter.
/// </summary>
public sealed class JsonConverterException : WrapException
{
	/// <summary>Create exception.</summary>
	/// <param name="message">Exception message.</param>
	public JsonConverterException(string message) : base(message) { }
}
