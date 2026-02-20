// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

internal static class C
{
	internal const string NoneFailureMessage = "Maybe<{Type}> was 'None'.";

	internal const string NullValueFailureMessage = "Null value of type '{Type}'.";

	internal const string TestFalseMessage = "Test returned false.";

	internal static class GetSingle
	{
		internal const string EmptyList =
			"Cannot get single value from an empty list.";

		internal const string MultipleValues =
			"Cannot get single value from a list with multiple values.";

		internal const string IncorrectType =
			"Cannot get single value of type '{ValueType}' from a list type '{ListType}'.";

		internal const string NotAList =
			"Result value type is not a list.";
	}
}
