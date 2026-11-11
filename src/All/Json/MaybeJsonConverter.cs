// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// <see cref="Maybe{T}"/> JSON converter.
/// </summary>
/// <typeparam name="T">Maybe value type.</typeparam>
public sealed class MaybeJsonConverter<T> : JsonConverter<Maybe<T>>
{
	/// <summary>
	/// Pass JSON null to <see cref="Read"/> so it becomes <see cref="None"/> instead of a null reference.
	/// </summary>
	public override bool HandleNull =>
		true;

	/// <summary>
	/// Read value and return as <see cref="Some{T}"/> or <see cref="None"/>.
	/// </summary>
	/// <param name="reader">Utf8JsonReader.</param>
	/// <param name="typeToConvert">Maybe with value type <typeparamref name="T"/>.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override Maybe<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// JSON null is None
		if (reader.TokenType == JsonTokenType.Null)
		{
			return M.None;
		}

		try
		{
			// Attempt to deserialise the value
			return JsonSerializer.Deserialize<T>(ref reader, options) switch
			{
				T x =>
					x,

				_ =>
					throw new NullMaybeValueException()
			};
		}
		catch (Exception e)
		{
			// When TValue is a value type we can create a blank object
			if (typeof(T).IsValueType && default(T) is T v)
			{
				reader.Skip(); // tell the reader we didn't read anything successfully
				return M.Wrap(v);
			}

			// Handle null input
			if (e.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
			{
				throw new NullMaybeValueException();
			}

			// Throw original exception
			throw;
		}
	}

	/// <summary>
	/// If <paramref name="value"/> is <see cref="Some{T}"/> write <see cref="Some{T}.Value"/>, otherwise write null
	/// (so <see cref="None"/> reads back as <see cref="None"/>).
	/// </summary>
	/// <param name="writer">Utf8JsonWriter.</param>
	/// <param name="value">Maybe object to be converted.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override void Write(Utf8JsonWriter writer, Maybe<T> value, JsonSerializerOptions options) =>
		M.Match(value ?? M.None, // HandleNull is true, so a null Maybe property arrives here too
			fNone: writer.WriteNullValue,
			fSome: x => JsonSerializer.Serialize(writer, x, options)
		);
}
