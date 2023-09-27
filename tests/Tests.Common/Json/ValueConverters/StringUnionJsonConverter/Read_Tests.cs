// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json.ValueConverters.StringUnionJsonConverter_Tests;

public class when_json_is_valid
{
	[Fact]
	public void deserialize_returns_union_with_value()
	{
		// Arrange
		var value = Rnd.Str;
		var json = $"\"{value}\"";
		var opt = new JsonSerializerOptions();
		opt.Converters.Add(new UnionJsonConverterFactory());

		// Act
		var result = JsonSerializer.Deserialize<Test>(json, opt);

		// Assert
		Assert.Equal(value, result!.Value);
	}
}

public sealed record class Test : Union<Test, string>;
