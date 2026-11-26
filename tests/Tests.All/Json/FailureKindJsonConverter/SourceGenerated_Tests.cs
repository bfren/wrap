// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wrap.Json.FailureKindJsonConverter_Tests;

// In Tests.All because Wrap.Result's internals are not visible here, so this sees what a real consumer sees
public class SourceGenerated_Tests
{
	[Fact]
	public void Context_Round_Trips_Failure_Kind()
	{
		// Arrange
		var failure = R.Fail(Rnd.Str).withKind(FailureKind.NotFound);

		// Act
		var written = JsonSerializer.Serialize(new Payload(failure), PayloadContext.Default.Payload);
		var read = JsonSerializer.Deserialize(written, PayloadContext.Default.Payload);

		// Assert
		Assert.Contains("\"Kind\":\"NotFound\"", written);
		Assert.Equal(FailureKind.NotFound, read?.Error.Kind);
	}
}

public sealed record Payload(Failure Error);

[JsonSerializable(typeof(Payload))]
public sealed partial class PayloadContext : JsonSerializerContext;
