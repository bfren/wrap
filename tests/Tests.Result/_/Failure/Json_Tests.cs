// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Failure_Tests;

public class Json_Tests
{
	[Fact]
	public void With_Thrown_Exception__Serialises_Without_Exception()
	{
		// Arrange
		var message = Rnd.Str;
		Result<int> result;
		try
		{
			// a thrown exception has a TargetSite, which System.Text.Json refuses to write
			throw new InvalidOperationException(message);
		}
		catch (InvalidOperationException ex)
		{
			result = R.Fail<int>(ex);
		}

		// Act
		var json = JsonSerializer.Serialize(result);

		// Assert
		Assert.Contains(message, json);
		Assert.DoesNotContain(nameof(Failure.Exception), json);
	}
}
