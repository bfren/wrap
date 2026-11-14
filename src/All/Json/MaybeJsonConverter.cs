// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// Convert <see cref="Maybe{T}"/> values to and from JSON: <see cref="Some{T}"/> as its value, <see cref="None"/> as JSON <c>null</c>.
/// </summary>
/// <typeparam name="T">Some value type.</typeparam>
public sealed class MaybeJsonConverter<T> : JsonConverter<Maybe<T>>
{
	/// <summary>
	/// Gets <see langword="true"/>, so JSON <c>null</c> is passed to <see cref="Read"/> and becomes <see cref="None"/>.
	/// </summary>
	public override bool HandleNull =>
		true;

	/// <summary>
	/// Read a JSON value as a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert">Type to convert (<see cref="Maybe{T}"/>).</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns><see cref="None"/> for JSON <c>null</c>, otherwise <see cref="Some{T}"/> holding the value read.</returns>
	/// <exception cref="JsonException">The JSON is not a valid <typeparamref name="T"/>.</exception>
	/// <exception cref="NullMaybeValueException">The input has no JSON tokens, or the value reads as <see langword="null"/>.</exception>
	public override Maybe<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
	/// Write the value of a <see cref="Some{T}"/>, or JSON <c>null</c> for <see cref="None"/> (so it reads back as <see cref="None"/>).
	/// </summary>
	/// <param name="writer">JSON writer.</param>
	/// <param name="value">Maybe object to write.</param>
	/// <param name="options">Serialiser options.</param>
	public override void Write(Utf8JsonWriter writer, Maybe<T> value, JsonSerializerOptions options)
	{
		switch (value)
		{
			case Some<T> x:
				JsonSerializer.Serialize(writer, x.Value, options);
				break;

			case None:
				writer.WriteNullValue();
				break;

			// IDE0010 does not treat the union cases as complete, so it needs this arm
			default:
				break;
		}
	}
}
