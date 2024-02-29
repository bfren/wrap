// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text;
using System.Text.Json;
using Wrap.Exceptions;

namespace Wrap.Json.UnionJsonConverterTests;

public class when_json_is_valid
{
	[Fact]
	public void returns_union_with_value()
	{
		// Arrange
		var value = Rnd.Str;
		var json = $"\"{value}\"";
		var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
		var opt = new JsonSerializerOptions();
		var converter = new UnionJsonConverter<Test, string>();

		// Act
		var result = converter.Read(ref reader, typeof(Test), opt);

		// Assert
		var test = Assert.IsType<Test>(result);
		Assert.Equal(value, test.Value);
	}
}

public class when_json_value_is_empty
{
	[Fact]
	public void throws_NullUnionValueException()
	{
		// Arrange
		var value = Rnd.Lng;
		var json = string.Empty;
		var opt = new JsonSerializerOptions();
		var converter = new UnionJsonConverter<Test, string>();

		// Act
		Test? act()
		{
			var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
			return converter.Read(ref reader, typeof(Test), opt);
		}

		// Assert
		_ = Assert.ThrowsAny<NullUnionValueException>(act);
	}
}

public class when_json_value_is_null
{
	[Fact]
	public void throws_NullUnionValueException()
	{
		// Arrange
		var value = Rnd.Lng;
		var json = "null";
		var opt = new JsonSerializerOptions();
		var converter = new UnionJsonConverter<Test, string>();

		// Act
		Test? act()
		{
			var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
			return converter.Read(ref reader, typeof(Test), opt);
		}

		// Assert
		_ = Assert.ThrowsAny<NullUnionValueException>(act);
	}
}

public class when_json_value_is_incorrect_type
{
	[Fact]
	public void throws_IncorrectValueTypeException()
	{
		// Arrange
		var value = Rnd.Lng;
		var json = $"{value}";
		var opt = new JsonSerializerOptions();
		var converter = new UnionJsonConverter<Test, string>();

		// Act
		Test? act()
		{
			var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
			return converter.Read(ref reader, typeof(Test), opt);
		}

		// Assert
		_ = Assert.ThrowsAny<IncorrectValueTypeException<Test, string>>(act);
	}
}

public sealed record class Test : Union<Test, string>;
