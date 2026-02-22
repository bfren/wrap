// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json.Serialization;

namespace Wrap.Json.ListExtensions_Tests;

public class AddWrapConverters_Tests
{
	[Fact]
	public void Adds_MaybeJsonConverterFactory_And_MonadJsonConverterFactory()
	{
		// Arrange
		var list = Substitute.For<IList<JsonConverter>>();

		// Act
		list.AddWrapConverters();

		// Assert
		list.Received().Add(Arg.Any<MaybeJsonConverterFactory>());
		list.Received().Add(Arg.Any<MonadJsonConverterFactory>());
	}
}
