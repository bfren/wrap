// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// JSON converter factory that creates a <see cref="MonadJsonConverter{TMonad, TValue}"/> for single-case monads,
/// such as strongly typed IDs.
/// </summary>
public sealed class MonadJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Check whether <paramref name="typeToConvert"/> implements <see cref="IMonad{TMonad, TValue}"/> once.
	/// </summary>
	/// <param name="typeToConvert">Type to convert to and from JSON.</param>
	/// <returns><see langword="true"/> if <paramref name="typeToConvert"/> implements <see cref="IMonad{TMonad, TValue}"/> once, otherwise <see langword="false"/>.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		F.GetMonadTypes(typeToConvert, typeof(IMonad<,>)) is not (null, null);

	/// <summary>
	/// Create a <see cref="MonadJsonConverter{TMonad, TValue}"/> for <paramref name="typeToConvert"/>.
	/// </summary>
	/// <param name="typeToConvert">Monad type to convert to and from JSON.</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>A <see cref="MonadJsonConverter{TMonad, TValue}"/> for <paramref name="typeToConvert"/>.</returns>
	/// <exception cref="JsonConverterException"><paramref name="typeToConvert"/> is not a single-case monad, has no
	/// parameterless constructor, or the converter could not be created.</exception>
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
