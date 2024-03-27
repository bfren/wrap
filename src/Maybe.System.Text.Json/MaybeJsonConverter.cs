// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json;

/// <summary>
/// <see cref="Maybe{T}"/> JSON converter.
/// </summary>
/// <typeparam name="T">Maybe value type.</typeparam>
internal sealed class MaybeJsonConverter<T> : JsonConverter<Maybe<T>>
{
	/// <summary>
	/// Read value and return as <see cref="Some{T}"/> or <see cref="None"/>.
	/// </summary>
	/// <param name="reader">Utf8JsonReader.</param>
	/// <param name="typeToConvert">Maybe with value type <typeparamref name="T"/>.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override Maybe<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		JsonSerializer.Deserialize<T>(ref reader, options) switch
		{
			T value =>
				value,

			_ =>
				M.None
		};

	/// <summary>
	/// If <paramref name="value"/> is <see cref="Some{T}"/> write <see cref="Some{T}.Value"/>, otherwise write an empty value.
	/// </summary>
	/// <param name="writer">Utf8JsonWriter.</param>
	/// <param name="value">Maybe object to be converted.</param>
	/// <param name="options">JsonSerializerOptions.</param>
	public override void Write(Utf8JsonWriter writer, Maybe<T> value, JsonSerializerOptions options) =>
		value.Match(
			some: x => JsonSerializer.Serialize(writer, x, options),
			none: () => writer.WriteStringValue(string.Empty)
		);
}
