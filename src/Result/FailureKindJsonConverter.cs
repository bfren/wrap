// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap;

/// <summary>
/// Write a <see cref="FailureKind"/> to JSON as its name, and read it back.
/// </summary>
/// <remarks>
/// Public so a System.Text.Json source-generated <see cref="JsonSerializerContext"/> can create it from the
/// <see cref="JsonConverterAttribute"/> on <see cref="FailureKind"/>. You don't need to register it yourself.
/// </remarks>
public sealed class FailureKindJsonConverter : JsonConverter<FailureKind>
{
	/// <summary>
	/// Read a <see cref="FailureKind"/> from a JSON string or <see langword="null"/>.
	/// </summary>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert">Type to convert.</param>
	/// <param name="options">Serialiser options.</param>
	/// <returns>
	/// The kind with the name read, or <see cref="FailureKind.Unspecified"/> for <see langword="null"/> or a name that is
	/// empty or only whitespace.
	/// </returns>
	/// <remarks>
	/// An empty name is read as <see cref="FailureKind.Unspecified"/> because it would otherwise print the same but not be equal to it.
	/// </remarks>
	public override FailureKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		reader.GetString() switch
		{
			string name when !string.IsNullOrWhiteSpace(name) =>
				new(name),

			_ =>
				FailureKind.Unspecified
		};

	/// <summary>
	/// Write <paramref name="value"/> as its name, or <see langword="null"/> for <see cref="FailureKind.Unspecified"/>.
	/// </summary>
	/// <remarks>
	/// A name that is empty or only whitespace is also written as <see langword="null"/>, so it reads back the same as
	/// <see cref="Read"/> reads it: as <see cref="FailureKind.Unspecified"/>.
	/// </remarks>
	/// <param name="writer">JSON writer.</param>
	/// <param name="value">Failure kind.</param>
	/// <param name="options">Serialiser options.</param>
	public override void Write(Utf8JsonWriter writer, FailureKind value, JsonSerializerOptions options)
	{
		if (value.Name is string name && !string.IsNullOrWhiteSpace(name))
		{
			writer.WriteStringValue(name);
		}
		else
		{
			writer.WriteNullValue();
		}
	}
}
