// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Abstracts;

public abstract class Serialize_Tests<TId, TIdValue>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{
	public abstract void Test00_Serialize_Direct__Returns_Valid_Json();

	protected static void Test00(TIdValue value)
	{
		// Arrange
		var expected = JsonSerializer.Serialize(value);

		// Act
		var result = JsonSerializer.Serialize(new TId() { Value = value }, Helpers.Json.Options);

		// Assert
		Assert.Equal(expected, result);
	}

	public abstract void Test01_Serialize_Wrapped__Returns_Valid_Json();

	protected static void Test01(TIdValue value)
	{
		// Arrange
		var id = Rnd.Int;
		var wrapped = new TestWrappedId(id, new() { Value = value });
		var expected = $"{{\"Id\":{id},\"WrappedId\":{JsonSerializer.Serialize(value)}}}";

		// Act
		var result = JsonSerializer.Serialize(wrapped, Helpers.Json.Options);

		// Assert
		Assert.Equal(expected, result);
	}

	public sealed record class TestWrappedId(int Id, TId WrappedId);
}
