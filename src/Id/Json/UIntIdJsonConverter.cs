// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Ids;

namespace Wrap.Json;

/// <summary>
/// JSON converter for <see cref="UIntId{TId}"/> value types.
/// </summary>
/// <inheritdoc cref="IdJsonConverter{TId, TValue}"/>
internal sealed class UIntIdJsonConverter<TId> : IdJsonConverter<TId, uint>
	where TId : UIntId<TId>, new()
{
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
					reader.GetUInt32(),

				// Handle strings if strings are allowed
				JsonTokenType.String when (options.NumberHandling & JsonNumberHandling.AllowReadingFromString) != 0 =>
					M.ParseUInt32(reader.GetString()).Unwrap(() => 0U),

				// Handle default
				_ =>
					HandleSkip(reader.TrySkip(), 0)
			}
		};
}
