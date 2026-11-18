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

	[Failure("{Class} raised {Event}")]
	public static partial Failure KeywordParameters(string @class, int @event);

	// lower case because only keywords need '@': the generator must keep it on type and method names
#pragma warning disable IDE1006, CA1716 // Naming Styles, and names that match keywords
	public partial class @event
	{
		[Failure("Locked {Id}")]
		public static partial Failure @lock(int id);
	}
#pragma warning restore IDE1006, CA1716

	public partial interface IInInterface
	{
		[Failure("In interface")]
		public static partial Failure Nested();
	}

	public readonly partial union InUnion(int, string)
	{
		[Failure("In union")]
		public static partial Failure Nested();
	}

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
