// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Generators.FailureGenerator_Tests.Shadowed;
using Wrap.Logging;

namespace Wrap.Generators.FailureGenerator_Tests;

public class Shadowed_Tests
{
	[Fact]
	public void Namespace_Ending_In_Wrap__Generates_Method()
	{
		// Arrange
		var name = Rnd.Str;
		var id = Rnd.Int;

		// Act
		var result = ShadowedFailures.NoOrder(name, id);

		// Assert
		Assert.Equal("User {Name} has no order {Id}.", result.Message);
		Assert.Equal([name, id], result.Args);
		Assert.Equal(LogLevel.Error, result.Level);
		Assert.Equal(FailureKind.NotFound, result.Kind);
	}

	[Fact]
	public void Nullable_Parameter__Accepts_Null()
	{
		// Arrange

		// Act
		var result = ShadowedFailures.NoOrder(null, Rnd.Int);

		// Assert
		Assert.Null(result.Args[0]);
	}
}
