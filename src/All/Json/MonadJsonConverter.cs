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
	/// <summary>
	/// Read a JSON value as a <typeparamref name="TMonad"/>.
	/// </summary>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert">Type to convert (<typeparamref name="TMonad"/>).</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>A new <typeparamref name="TMonad"/> holding the value read.</returns>
	/// <exception cref="JsonException">The JSON is not a valid <typeparamref name="TValue"/>.</exception>
	/// <exception cref="NullMonadValueException">The input has no JSON tokens, or the value reads as <see langword="null"/>.</exception>
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
			// Handle null input
			if (e.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
			{
				throw new NullMonadValueException();
			}

			// Throw original exception
			throw;
		}
	}

	/// <summary>
	/// Write the wrapped value, or JSON <c>null</c> if <paramref name="value"/> is <see langword="null"/>.
	/// </summary>
	/// <param name="writer">JSON writer.</param>
	/// <param name="value">Monad object to write.</param>
	/// <param name="options">Serialiser options.</param>
	public override void Write(Utf8JsonWriter writer, TMonad value, JsonSerializerOptions options)
	{
		if (value is null)
		{
			writer.WriteNullValue();
			return;
		}

		JsonSerializer.Serialize(writer, value.Value, options);
	}
}
