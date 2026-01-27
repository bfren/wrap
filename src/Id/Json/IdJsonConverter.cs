// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json;

/// <summary>
/// <see cref="IId{TId, TValue}"/> JSON converter.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
/// <typeparam name="TValue">ID value type.</typeparam>
internal abstract class IdJsonConverter<TId, TValue> : JsonConverter<TId>
	where TId : Id<TId, TValue>, new()
	where TValue : struct
{
	/// <summary>
	/// Write an <see cref="IId{TId, TValue}"/> type value.
	/// </summary>
	/// <param name="writer">Json Writer.</param>
	/// <param name="value">ID value.</param>
	/// <param name="options">JSON options.</param>
	public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
		writer.WriteStringValue(value switch
		{
			TId id =>
				id.Value.ToString(),

			_ =>
				null
		});

	/// <summary>
	/// Try to skip the JSON token (because it hasn't been matched correctly) and return a default value.
	/// </summary>
	/// <param name="skipped">Whether or not the JSON token has been skipped.</param>
	/// <param name="defaultValue">Default value to use if token has been skipped.</param>
	/// <exception cref="JsonException"></exception>
	internal TValue HandleSkip(bool skipped, TValue defaultValue) =>
		skipped switch
		{
			true =>
				defaultValue,

			_ =>
				throw new JsonException($"Invalid {typeof(TValue)} and unable to skip reading current token.")
		};
}
