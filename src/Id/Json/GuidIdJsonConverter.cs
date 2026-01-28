// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using Wrap.Ids;

namespace Wrap.Json;

/// <summary>
/// JSON converter for <see cref="GuidId{TId}"/> value types.
/// </summary>
/// <inheritdoc cref="IdJsonConverter{TId, TValue}"/>
internal sealed class GuidIdJsonConverter<TId> : IdJsonConverter<TId, Guid>
	where TId : GuidId<TId>, new()
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
				// Handle strings
				JsonTokenType.String =>
					M.ParseGuid(reader.GetString()).Unwrap(() => Guid.Empty),

				// Handle default
				_ =>
					HandleSkip(reader.TrySkip(), Guid.Empty)
			}
		};
}
