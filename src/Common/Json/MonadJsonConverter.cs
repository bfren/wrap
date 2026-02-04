// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// Convert <see cref="Monad{TMonad, TValue}"/> types to and from JSON.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Monad value type.</typeparam>
public sealed class MonadJsonConverter<TMonad, TValue> : JsonConverter<TMonad>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TMonad value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.Value?.ToString());

	/// <inheritdoc/>
	public override TMonad? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		try
		{
			return JsonSerializer.Deserialize<TValue>(ref reader, options) switch
			{
				TValue x =>
					F.Wrap<TMonad, TValue>(x),

				_ =>
					throw new NullMonadValueException()
			};
		}
		catch (JsonException e)
		{
			if (e.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
			{
				throw new NullMonadValueException();
			}

			throw new IncorrectValueTypeException<TMonad, TValue>(e);
		}
	}
}
