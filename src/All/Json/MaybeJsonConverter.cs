// Wrap: .NET monads.
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
	/// Read value and return as <see cref="Some{T}"/> or <see cref="None"/>.
	/// </summary>
	/// <param name="reader">Utf8JsonReader.</param>
	/// <param name="typeToConvert">Maybe with value type <typeparamref name="T"/>.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override Maybe<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
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
			if (typeof(T).IsValueType)
			{
				reader.Skip(); // tell the reader we didn't read anything successfully
				return M.Wrap(default(T)!);
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
	/// If <paramref name="value"/> is <see cref="Some{T}"/> write <see cref="Some{T}.Value"/>, otherwise write an empty value.
	/// </summary>
	/// <param name="writer">Utf8JsonWriter.</param>
	/// <param name="value">Maybe object to be converted.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override void Write(Utf8JsonWriter writer, Maybe<T> value, JsonSerializerOptions options) =>
		M.Match(value,
			fSome: x => JsonSerializer.Serialize(writer, x, options),
			fNone: () => writer.WriteStringValue(string.Empty)
		);
}
