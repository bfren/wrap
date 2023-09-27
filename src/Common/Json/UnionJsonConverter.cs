// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json;

/// <summary>
/// Convert <see cref="Union{TUnion, TValue}"/> types to and from JSON.
/// </summary>
/// <typeparam name="TUnion">Union type.</typeparam>
/// <typeparam name="TValue">Union value type.</typeparam>
internal sealed class UnionJsonConverter<TUnion, TValue> : JsonConverter<TUnion>
	where TUnion : IUnion<TValue>, new()
{
	/// <inheritdoc/>
	public override void Write(Utf8JsonWriter writer, TUnion value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value.Value?.ToString());

	/// <inheritdoc/>
	public override TUnion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		F.Wrap<TUnion, TValue>(JsonSerializer.Deserialize<TValue>(ref reader, options));
}
