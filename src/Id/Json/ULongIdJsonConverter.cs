// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Ids;

namespace Wrap.Json;

/// <summary>
/// JSON converter for <see cref="ULongId{TId}"/> value types.
/// </summary>
/// <inheritdoc cref="IdJsonConverter{TId, TValue}"/>
internal sealed class ULongIdJsonConverter<TId> : IdJsonConverter<TId, ulong>
	where TId : ULongId<TId>, new()
{
	/// <summary>
	/// Write an <see cref="IId{TId, TValue}"/> type value.
	/// </summary>
	/// <param name="writer">Json Writer.</param>
	/// <param name="value">ID value.</param>
	/// <param name="options">JSON options.</param>
	public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options) =>
		writer.WriteNumberValue(value.Value);

	/// <summary>
	/// Read <see cref="GuidId{TId}"/> type value.
	/// </summary>
	/// <param name="reader">JSON reader.</param>
	/// <param name="typeToConvert"><typeparamref name="TId"/> type.</param>
	/// <param name="options">JSON options.</param>
	public override TId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		new()
		{
			Value = reader.TokenType switch
			{
				// Handle numbers
				JsonTokenType.Number =>
					reader.GetUInt64(),

				// Handle strings if strings are allowed
				JsonTokenType.String when (options.NumberHandling & JsonNumberHandling.AllowReadingFromString) != 0 =>
					M.ParseUInt64(reader.GetString()).Unwrap(() => 0UL),

				// Handle default
				_ =>
					HandleSkip(reader.TrySkip(), 0)
			}
		};
}
