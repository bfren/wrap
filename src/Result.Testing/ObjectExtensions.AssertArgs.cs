// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Testing;

public static partial class ObjectExtensions
{
	/// <summary>
	/// Assert that an anonymous args object matches what is expected.
	/// </summary>
	/// <param name="this">Expected args object.</param>
	/// <param name="actual">Actual args object.</param>
	public static void AssertArgs(this object @this, object? actual)
	{
		// Fail if args is null
		Assert.NotNull(actual);

		// Serialise to perform case-insensitive comparison
		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		Assert.Equal(JsonSerializer.Serialize(@this, options), JsonSerializer.Serialize(actual, options));
	}
}
