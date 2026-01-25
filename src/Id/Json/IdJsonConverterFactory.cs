// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// <see cref="IId{TId, TValue}"/> JSON converter factory.
/// </summary>
public sealed class IdJsonConverterFactory : JsonConverterFactory
{
	/// <summary>
	/// Returns true if <paramref name="typeToConvert"/> is a <see cref="IUnion{TUnion, TValue}"/>.
	/// </summary>
	/// <param name="typeToConvert"><see cref="IId{TId, TValue}"/> type.</param>
	public override bool CanConvert(Type typeToConvert) =>
		F.GetUnionTypes(typeToConvert, typeof(IId<,>)) is not (null, null);

	/// <summary>
	/// Creates JsonConverter using <paramref name="typeToConvert"/> type as generic argument.
	/// </summary>
	/// <param name="typeToConvert"><see cref="IId{TId, TValue}"/> type.</param>
	/// <param name="options">JSON options.</param>
	/// <exception cref="JsonConverterException"></exception>
	public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
	{
		// IId<,> requires two type arguments
		var (idType, idValueType) = F.GetUnionTypes(typeToConvert, typeof(IId<,>));
		if (idType is null || idValueType is null)
		{
			throw new JsonConverterException(
				$"{typeToConvert} is an invalid {typeof(IId<,>)}: " +
				"please implement one of the provided abstract ID record types."
			);
		}

		// Ensure there is a parameterless contstructor
		if (typeToConvert.GetConstructor([]) is null)
		{
			throw new JsonConverterException(
				$"{typeToConvert} does not have a parameterless constructor."
			);
		}

		// Use the Value type to determine which converter to use
		var idConverter = idValueType switch
		{
			Type t when t == typeof(Guid) =>
				typeof(GuidIdJsonConverter<>),

			Type t when t == typeof(int) =>
				typeof(IntIdJsonConverter<>),

			Type t when t == typeof(long) =>
				typeof(LongIdJsonConverter<>),

			Type t when t == typeof(uint) =>
				typeof(UIntIdJsonConverter<>),

			Type t when t == typeof(ulong) =>
				typeof(ULongIdJsonConverter<>),

			{ } t =>
				throw new JsonConverterException(
					$"StrongId with value type {t} is not supported."
				)
		};

		// Attempt to create and return the converter
		var genericType = idConverter.MakeGenericType(typeToConvert);
		return Activator.CreateInstance(genericType) switch
		{
			JsonConverter x =>
				x,

			_ =>
				throw new JsonConverterException(
					$"Unable to create {typeof(IdJsonConverter<,>)} for type {typeToConvert}."
				)
		};
	}
}
