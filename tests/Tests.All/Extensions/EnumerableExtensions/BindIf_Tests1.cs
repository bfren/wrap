// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class BindIf_Tests
{
	internal static void AssertFailures<T>(IEnumerable<Result<T>> actual) =>
		Assert.Collection(actual,
			x => x.AssertFailure(C.PredicateFalseMessage),
			x => x.AssertFailure(C.PredicateFalseMessage),
			x => x.AssertFailure(C.PredicateFalseMessage)
		);
}
