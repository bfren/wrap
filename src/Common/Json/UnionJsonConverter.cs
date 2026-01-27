// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Exceptions;

namespace Wrap.Json;

/// <summary>
/// Convert <see cref="Union{TUnion, TValue}"/> types to and from JSON.
/// </summary>
/// <typeparam name="TUnion">Union type.</typeparam>
/// <typeparam name="TValue">Union value type.</typeparam>
public sealed class UnionJsonConverter<TUnion, TValue> : JsonConverter<TUnion>
	where TUnion : IUnion<TUnion, TValue>, new()
{
	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TUnion value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.Value?.ToString());

	/// <inheritdoc/>
	public override TUnion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		try
		{
			return JsonSerializer.Deserialize<TValue>(ref reader, options) switch
			{
				TValue x =>
					F.Wrap<TUnion, TValue>(x),

				_ =>
					throw new NullUnionValueException()
			};
		}
		catch (JsonException e)
		{
			if (e.Message.StartsWith("The input does not contain any JSON tokens.", StringComparison.OrdinalIgnoreCase))
			{
				throw new NullUnionValueException();
			}

			throw new IncorrectValueTypeException<TUnion, TValue>(e);
		}
	}
}
