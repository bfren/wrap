// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using Wrap.Ids;

namespace Wrap.Json.IdJsonConverter_Tests;

public class TrySkip_Tests
{
	[Fact]
	public void Skipped_True__Returns_DefaultValue()
	{
		// Arrange
		var defaultValue = Rnd.Guid;
		var converter = Substitute.ForPartsOf<IdJsonConverter<TestGuidId, Guid>>();

		// Act
		var result = converter.HandleSkip(true, defaultValue);

		// Assert
		Assert.Equal(defaultValue, result);
	}

	[Fact]
	public void Skipped_False__Throws_JsonException()
	{
		// Arrange
		var converter = Substitute.ForPartsOf<IdJsonConverter<TestLongId, long>>();

		// Act
		var result = Record.Exception(() => converter.HandleSkip(false, Rnd.Lng));

		// Assert
		var ex = Assert.IsType<JsonException>(result);
		Assert.Equal($"Invalid {typeof(long)} and unable to skip reading current token.", ex.Message);
	}

	public sealed record class TestGuidId : GuidId<TestGuidId>;

	public sealed record class TestLongId : LongId<TestLongId>;
}
