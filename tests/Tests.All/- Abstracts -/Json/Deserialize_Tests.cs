// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Abstracts;

public abstract class Deserialize_Tests<TId, TIdValue>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{
	public abstract void Test00_Deserialise__Valid_Json__Returns_Id_With_Value(string format);

	protected static void Test00(string format, TIdValue value)
	{
		// Arrange
		var json = string.Format(format, value);

		// Act
		var result = JsonSerializer.Deserialize<TId>(json, Helpers.Json.Options);

		// Assert
		Assert.Equal(value, result!.Value);
	}

	public abstract void Test01_Deserialise__Valid_Json__Returns_Object_With_Id_Value(string format);

	protected static void Test01(string format, TIdValue value)
	{
		// Arrange
		var id = Rnd.Int;
		var json = $"{{ \"Id\": {id}, \"WrappedId\": {string.Format(format, value)} }}";

		// Act
		var result = JsonSerializer.Deserialize<TestWrappedId>(json, Helpers.Json.Options);

		// Assert
		Assert.Equal(id, result!.Id);
		Assert.Equal(value, result.WrappedId.Value);
	}

	public abstract void Test02_Deserialise__Invalid_Json__Throws_JsonException(string input);

	protected static void Test02(string input)
	{
		// Arrange

		// Act
		var result = Record.Exception(() => JsonSerializer.Deserialize<TId>(input, Helpers.Json.Options));

		// Assert
		Assert.IsType<JsonException>(result);
	}

	public abstract void Test03_Deserialise__Invalid_Json_In_Object__Throws_JsonException(string input);

	protected static void Test03(string input)
	{
		// Arrange
		var json = $"{{ \"Id\": {Rnd.Int}, \"WrappedId\": {input} }}";

		// Act
		var result = Record.Exception(() => JsonSerializer.Deserialize<TestWrappedId>(json, Helpers.Json.Options));

		// Assert
		Assert.IsType<JsonException>(result);
	}

	public sealed record class TestWrappedId(int Id, TId WrappedId);
}
