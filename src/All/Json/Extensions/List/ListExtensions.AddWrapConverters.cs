// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Wrap.Json;

public static partial class ListExtensions
{
	/// <summary>
	/// Add <see cref="MaybeJsonConverterFactory"/> and
	/// <see cref="MonadJsonConverterFactory"/> to the list of JSON converters.
	/// </summary>
	/// <param name="this">List of JsonConverters.</param>
	public static void AddWrapConverters(this IList<JsonConverter> @this)
	{
		@this.Add(new MaybeJsonConverterFactory());
		@this.Add(new MonadJsonConverterFactory());
	}
}
