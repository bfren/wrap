// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json;

/// <summary>
/// Extension methods for <see cref="JsonSerializerOptions"/> objects.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
	/// <summary>
	/// Add <see cref="UnionJsonConverterFactory"/> to the list of converters.
	/// </summary>
	/// <param name="this">JSON serializer options.</param>
	public static void AddUnionConverter(this JsonSerializerOptions @this) =>
		@this.Converters.Add(new UnionJsonConverterFactory());
}
