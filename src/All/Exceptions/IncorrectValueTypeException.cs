// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using Wrap.Json;

namespace Wrap.Exceptions;

/// <summary>
/// Thrown when <see cref="MonadJsonConverter{TMonad, TValue}.Read(ref Utf8JsonReader, System.Type, JsonSerializerOptions)"/>
/// encounters a value of the wrong type.
/// </summary>
public sealed class IncorrectValueTypeException<TMonad, TExpectedValue>(JsonException inner) :
	WrapException($"Monad type {typeof(TMonad)} expects a value of type {typeof(TExpectedValue)}.", inner);
