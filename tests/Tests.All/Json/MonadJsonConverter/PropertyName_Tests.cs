// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using Wrap.Ids;

namespace Wrap.Json.MonadJsonConverter_Tests;

public class PropertyName_Tests
{
	private static JsonSerializerOptions Options
	{
		get
		{
			var options = new JsonSerializerOptions();
			options.Converters.AddWrapConverters();
			return options;
		}
	}

	[Fact]
	public void Int_Id_Key__Round_Trips()
	{
		// Arrange
		var (id, value) = (IdGen.IntId<TestIntId>(), Rnd.Str);
		var input = new Dictionary<TestIntId, string> { [id] = value };

		// Act
		var json = JsonSerializer.Serialize(input, Options);
		var result = JsonSerializer.Deserialize<Dictionary<TestIntId, string>>(json, Options);

		// Assert
		Assert.Equal($"{{\"{id.Value}\":\"{value}\"}}", json);
		Assert.Equal(input, result);
	}

	[Fact]
	public void Guid_Id_Key__Round_Trips()
	{
		// Arrange
		var (id, value) = (IdGen.GuidId<TestGuidId>(), Rnd.Int);
		var input = new Dictionary<TestGuidId, int> { [id] = value };

		// Act
		var json = JsonSerializer.Serialize(input, Options);
		var result = JsonSerializer.Deserialize<Dictionary<TestGuidId, int>>(json, Options);

		// Assert
		Assert.Equal(input, result);
	}

	[Fact]
	public void Invalid_Key__Throws_JsonException()
	{
		// Arrange
		var json = $"{{\"{Rnd.Str}\":1}}";

		// Act
		var result = Record.Exception(() => JsonSerializer.Deserialize<Dictionary<TestIntId, int>>(json, Options));

		// Assert
		_ = Assert.IsType<JsonException>(result);
	}

	public sealed record class TestIntId : IntId<TestIntId>;

	public sealed record class TestGuidId : GuidId<TestGuidId>;
}
