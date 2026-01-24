// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using Wrap.Json.Converters;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="UnionJsonConverter{TUnion, TValue}.Read(ref Utf8JsonReader, System.Type, JsonSerializerOptions)"/>
/// encounters a value of the wrong type.
/// </summary>
public sealed class IncorrectValueTypeException<TUnion, TExpectedValue>(JsonException inner) :
	WrapException($"Union type {typeof(TUnion)} expects a value of type {typeof(TExpectedValue)}.", inner)
{ }
