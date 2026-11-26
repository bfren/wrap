// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

// A namespace ending in '.Wrap' hides the real Wrap namespace from code inside its parent, so generated code must use global::
namespace Wrap.Generators.FailureGenerator_Tests.Shadowed
{
	public static partial class ShadowedFailures
	{
		[Failure("User {Name} has no order {Id}", LogLevel.Error, Kind = nameof(FailureKind.NotFound))]
		public static partial Failure NoOrder(string? name, int id);
	}
}

namespace Wrap.Generators.FailureGenerator_Tests.Shadowed.Wrap
{
	internal static class Marker;
}
