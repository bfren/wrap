// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json;

/// <summary>
/// <see cref="Maybe{T}"/> JSON converter factory.
/// </summary>
internal sealed class MaybeJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Returns true if <paramref name="typeToConvert"/> is <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="typeToConvert">Type to convert.</param>
	/// <returns>Whether or not <paramref name="typeToConvert"/> can be converted to / from a <see cref="Maybe{T}"/> object.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		typeToConvert.IsGenericType && (typeToConvert.GetGenericTypeDefinition() == typeof(Maybe<>));

	/// <summary>
	/// Creates JsonConverter for <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="typeToConvert">Maybe type to be converted to / from JSON.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	/// <exception cref="JsonException"/>
	/// <returns><see cref="MaybeJsonConverter{T}"/> object.</returns>
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		// Get converter type
		var wrappedType = typeToConvert.GetGenericArguments()[0];
		var converterType = typeof(MaybeJsonConverter<>).MakeGenericType(wrappedType);

		// Create converter
		return Activator.CreateInstance(converterType) switch
		{
			JsonConverter x =>
				x,

			_ =>
				throw new JsonException($"Unable to create {converterType} for type {typeToConvert}.")
		};
	}
}
