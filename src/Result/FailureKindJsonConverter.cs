// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap;

/// <summary>
/// Write a <see cref="FailureKind"/> to JSON as its name, and read it back.
/// </summary>
internal sealed class FailureKindJsonConverter : JsonConverter<FailureKind>
{
	/// <summary>
	/// Read a <see cref="FailureKind"/> from a JSON string or <see langword="null"/>.
	/// </summary>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert">Type to convert.</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>The kind with the name read, or <see cref="FailureKind.Unspecified"/> for <see langword="null"/>.</returns>
	public override FailureKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		new(reader.GetString());

	/// <summary>
	/// Write <paramref name="value"/> as its name, or <see langword="null"/> for <see cref="FailureKind.Unspecified"/>.
	/// </summary>
	/// <param name="writer">JSON writer.</param>
	/// <param name="value">Failure kind.</param>
	/// <param name="options">Serialiser options.</param>
	public override void Write(Utf8JsonWriter writer, FailureKind value, JsonSerializerOptions options)
	{
		if (value.Name is string name)
		{
			writer.WriteStringValue(name);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
