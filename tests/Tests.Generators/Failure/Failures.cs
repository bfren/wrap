// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;

namespace Wrap.Generators.FailureValueGenerator_Tests;

public static partial class Failures
{
	[Failure("User {UserId} was not found")]
	public static partial FailureValue NoLevel(int userId);

	[Failure("Value \"{Value}\" is not a C:\\ path", LogLevel.Error)]
	internal static partial FailureValue Escaped(string value);

	public partial record struct InRecordStruct
	{
		[Failure("Nested")]
		public static partial FailureValue Nested();
	}
}
