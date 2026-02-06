// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

internal static class C
{
	internal const string NoneFailureMessage = "Maybe<{Type}> was 'None'.";

	internal const string NullValueFailureMessage = "Null value of type '{Type}'.";

	internal const string PredicateFalseMessage = "Predicate was false.";
}
