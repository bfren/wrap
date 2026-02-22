// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// Convert <see cref="IMonad{TMonad, TValue}"/> types to and from JSON.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Monad value type.</typeparam>
public sealed class MonadJsonConverter<TMonad, TValue> : JsonConverter<TMonad>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <inheritdoc/>
	public override TMonad? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		try
		{
			// Attempt to deserialise the value
			return JsonSerializer.Deserialize<TValue>(ref reader, options) switch
			{
				TValue x =>
					F.Wrap<TMonad, TValue>(x),

				_ =>
					throw new NullMonadValueException()
			};
		}
		catch (Exception e)
		{
			// When TValue is a value type we can create a blank object
			if (typeof(TValue).IsValueType)
			{
				reader.Skip(); // tell the reader we didn't read anything successfully
				return new();
			}

			// Handle null input
			if (e.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
			{
				throw new NullMonadValueException();
			}

			// Throw original exception
			throw;
		}
	}

	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TMonad value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
			return;
		}

		var json = JsonSerializer.SerializeToUtf8Bytes(value.Value, options);
		writer.WriteRawValue(json);
	}
}
