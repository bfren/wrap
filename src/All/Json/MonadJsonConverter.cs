// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;
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

	/// <summary>
	/// Read a JSON property name as a <typeparamref name="TMonad"/>, so it can be a dictionary key.
	/// </summary>
	/// <remarks>
	/// The name is read by the converter for <typeparamref name="TValue"/>, so it must be a type that can be a dictionary key itself.
	/// </remarks>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert">Type to convert (<typeparamref name="TMonad"/>).</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>A new <typeparamref name="TMonad"/> holding the value read.</returns>
	/// <exception cref="NullMonadValueException">The name reads as <see langword="null"/>.</exception>
	public override TMonad ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		F.Wrap<TMonad, TValue>(ValueConverter(options).ReadAsPropertyName(ref reader, typeof(TValue), options));

	/// <summary>
	/// Write the wrapped value as a JSON property name, so <typeparamref name="TMonad"/> can be a dictionary key.
	/// </summary>
	/// <inheritdoc cref="ReadAsPropertyName" path="/remarks"/>
	/// <param name="writer">JSON writer.</param>
	/// <param name="value">Monad object to write.</param>
	/// <param name="options">Serialiser options.</param>
	public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] TMonad value, JsonSerializerOptions options) =>
		ValueConverter(options).WriteAsPropertyName(writer, value.Value!, options);

	private static JsonConverter<TValue> ValueConverter(JsonSerializerOptions options) =>
		(JsonConverter<TValue>)options.GetConverter(typeof(TValue));
}
