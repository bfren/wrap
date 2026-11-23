// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class FailureExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> has the expected kind.
	/// </summary>
	/// <param name="this">Failure object.</param>
	/// <param name="kind">Expected failure kind.</param>
	/// <returns><paramref name="this"/>, so assertions can be chained.</returns>
	public static Failure AssertKind(this Failure @this, FailureKind kind)
	{
		Assert.Equal(kind, @this.Kind);
		return @this;
	}
}
