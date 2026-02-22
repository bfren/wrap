// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Some{T}"/> and the value is true.
	/// </summary>>
	/// <param name="this">Maybe object.</param>
	public static void AssertTrue(this Maybe<bool> @this) =>
		Assert.True(@this.AssertSome());
}
