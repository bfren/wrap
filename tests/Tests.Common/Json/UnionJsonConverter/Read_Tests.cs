// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text;
using System.Text.Json;
using Wrap.Exceptions;

namespace Wrap.Json.UnionJsonConverter_Tests;

public class Read_Tests
{
	[Fact]
	public void return_union_with_value__when_json_is_valid()
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

	[Fact]
	public void throw_NullUnionValueException__when_json_value_is_empty_string()
	{
		// Arrange
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

	[Fact]
	public void throw_NullUnionValueException__when_json_value_is_null()
	{
		// Arrange
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

	[Fact]
	public void throw_IncorrectValueTypeException__when_json_value_is_incorrect_type()
	{
		// Arrange
		var value = Rnd.IntPtr;
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

	public sealed record class Test() : Union<Test, string>(Rnd.Str);
}
