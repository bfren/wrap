// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

// in the global namespace on purpose: the generated code must not add a namespace, or this does not compile
#pragma warning disable CA1050

public static partial class GlobalFailures
{
	[Failure("Global")]
	public static partial Failure Global();
}
