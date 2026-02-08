// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// Create a <see cref="MonadJsonConverter{TMonad, TValue}"/> object for supported objects.
/// </summary>
public sealed class MonadJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Returns true if <see cref="MonadJsonConverterFactory"/> can convert <paramref name="typeToConvert"/>.
	/// </summary>
	/// <param name="typeToConvert">The type to convert to / from JSON.</param>
	/// <returns>Whether or not <paramref name="typeToConvert"/> implements <see cref="IMonad{TMonad,TValue}"/>.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		F.GetMonadTypes(typeToConvert, typeof(IMonad<,>)) is not (null, null);

	/// <inheritdoc/>
	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		// IMonad<,> requires two type arguments
		var (monadType, valueType) = F.GetMonadTypes(typeToConvert, typeof(IMonad<,>));
		if (monadType is null || valueType is null)
		{
			throw new JsonConverterException(
				$"{typeToConvert} is an invalid {typeof(IMonad<,>)}."
			);
		}

		// Ensure there is a parameterless contstructor
		if (typeToConvert.GetConstructor([]) is null)
		{
			throw new JsonConverterException(
				$"{typeToConvert} does not have a parameterless constructor."
			);
		}

		// Attempt to create and return the converter
		var genericType = typeof(MonadJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
		return Activator.CreateInstance(genericType) switch
		{
			JsonConverter converter =>
				converter,

			_ =>
				throw new JsonConverterException(
					$"Unable to create {typeof(MonadJsonConverter<,>)} for type {typeToConvert}."
				)
		};
	}
}
