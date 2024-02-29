// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

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
	/// <returns>Whether or not <paramref name="typeToConvert"/> implements <see cref="IUnion{T}"/>.</returns>
	public override bool CanConvert(Type typeToConvert) =>
		typeToConvert.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUnion<>));

	/// <inheritdoc/>
	public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		// Get the Union Value type
		var valueTypeQuery = from i in typeToConvert.GetInterfaces()
							 where i.IsGenericType
							 && i.GetGenericTypeDefinition() == typeof(IUnion<>)
							 select i.GenericTypeArguments[0];
		var valueType = valueTypeQuery.SingleOrDefault();
		if (valueType is null)
		{
			return null;
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
