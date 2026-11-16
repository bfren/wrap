// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

// parameters are used by the generated implementation, which dotnet format cannot see before the generator is built
#pragma warning disable IDE0060

using Wrap.Logging;

namespace Wrap.Generators.FailureGenerator_Tests;

public static partial class Failures
{
	[Failure("User {UserId} was not found")]
	public static partial Failure NoLevel(int userId);

	[Failure("Value \"{Value}\" is not a C:\\ path", LogLevel.Error)]
	internal static partial Failure Escaped(string value);

	[Failure("Item {Id} was not found")]
	public static partial Failure Overload(int id);

	[Failure("Item {Name} was not found")]
	public static partial Failure Overload(string name);

	[Failure("Names {Names} were not found")]
	public static partial Failure ArrayParameter(string[] names);

	[Failure("{Message} in {Context}")]
	public static partial Failure ParameterNames(string message, string context);

	public partial record struct InRecordStruct
	{
		[Failure("Nested")]
		public static partial Failure Nested();
	}

	public partial class InGeneric<T>
		where T : struct
	{
		[Failure("Bad {Value}")]
		public static partial Failure Bad(T value);
	}
}
