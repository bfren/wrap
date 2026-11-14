// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json;

/// <summary>
/// JSON converter factory that creates a <see cref="MaybeJsonConverter{T}"/> for each <see cref="Maybe{T}"/> type.
/// </summary>
public sealed class MaybeJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Check whether <paramref name="typeToConvert"/> is a <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="typeToConvert">Type to convert.</param>
	/// <returns><see langword="true"/> if <paramref name="typeToConvert"/> is a <see cref="Maybe{T}"/>, otherwise <see langword="false"/>.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		typeToConvert.IsGenericType && (typeToConvert.GetGenericTypeDefinition() == typeof(Maybe<>));

	/// <summary>
	/// Create a <see cref="MaybeJsonConverter{T}"/> for <paramref name="typeToConvert"/>.
	/// </summary>
	/// <param name="typeToConvert">Maybe type to convert to and from JSON.</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>A <see cref="MaybeJsonConverter{T}"/> for the Some value type of <paramref name="typeToConvert"/>.</returns>
	/// <exception cref="JsonException">The converter could not be created.</exception>
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
