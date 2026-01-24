// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;
using Wrap.Json.Converters;

namespace Wrap.Json;

/// <summary>
/// Create a <see cref="UnionJsonConverter{TUnion, TValue}"/> object for supported objects.
/// </summary>
public sealed class UnionJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Returns true if <see cref="UnionJsonConverterFactory"/> can convert <paramref name="typeToConvert"/>.
	/// </summary>
	/// <param name="typeToConvert">The type to convert to / from JSON.</param>
	/// <returns>Whether or not <paramref name="typeToConvert"/> implements <see cref="IUnion{TUnion,TValue}"/>.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		F.GetUnionTypes(typeToConvert, typeof(IUnion<,>)) is not (null, null);

	/// <inheritdoc/>
	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		// IUnion<,> requires two type arguments
		var (unionType, valueType) = F.GetUnionTypes(typeToConvert, typeof(IUnion<,>));
		if (unionType is null || valueType is null)
		{
			throw new JsonConverterException(
				$"{typeToConvert} is an invalid {typeof(IUnion<,>)}."
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
		var genericType = typeof(UnionJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
		return Activator.CreateInstance(genericType) switch
		{
			JsonConverter converter =>
				converter,

			_ =>
				throw new JsonConverterException(
					$"Unable to create {typeof(UnionJsonConverter<,>)} for type {typeToConvert}."
				)
		};
	}
}
