// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json;

/// <summary>
/// Extension methods for <see cref="JsonSerializerOptions"/> objects.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
	/// <summary>
	/// Add <see cref="MaybeJsonConverterFactory"/> and
	/// <see cref="MonadJsonConverterFactory"/> to the list of JSON converters.
	/// </summary>
	/// <param name="this">JSON serializer options.</param>
	public static void AddWrapConverters(this JsonSerializerOptions @this)
	{
		@this.Converters.Add(new MaybeJsonConverterFactory());
		@this.Converters.Add(new MonadJsonConverterFactory());
	}
}
