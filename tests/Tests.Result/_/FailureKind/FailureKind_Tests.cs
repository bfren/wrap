// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.FailureKind_Tests;

public class FailureKind_Tests
{
	[Fact]
	public void Default_Is_Unspecified()
	{
		// Arrange

		// Act
		var result = default(FailureKind);

		// Assert
		Assert.Equal(FailureKind.Unspecified, result);
		Assert.Null(result.Name);
		Assert.Equal(string.Empty, result.ToString());
	}

	[Fact]
	public void Well_Known_Kind_Name_Is_Member_Name()
	{
		// Arrange

		// Act
		var result = FailureKind.NotFound;

		// Assert
		Assert.Equal(nameof(FailureKind.NotFound), result.Name);
		Assert.Equal(nameof(FailureKind.NotFound), result.ToString());
	}

	[Theory]
	[InlineData("NotFound", "\"NotFound\"")]
	[InlineData(null, "null")]
	public void Json_Round_Trips_As_Name(string? name, string json)
	{
		// Arrange
		var kind = new FailureKind(name);

		// Act
		var written = JsonSerializer.Serialize(kind);
		var read = JsonSerializer.Deserialize<FailureKind>(written);

		// Assert
		Assert.Equal(json, written);
		Assert.Equal(kind, read);
	}

	[Theory]
	[InlineData("\"\"")]
	[InlineData("\"   \"")]
	public void Json_Empty_Name_Reads_As_Unspecified(string json)
	{
		// Arrange

		// Act
		var result = JsonSerializer.Deserialize<FailureKind>(json);

		// Assert
		Assert.Equal(FailureKind.Unspecified, result);
	}

	[Fact]
	public void Json_Name_Ignores_Naming_Policy()
	{
		// Arrange
		var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

		// Act
		var result = JsonSerializer.Serialize(R.Fail(Rnd.Str).Knd(FailureKind.NotFound), options);

		// Assert
		Assert.Contains("\"kind\":\"NotFound\"", result);
	}
}
