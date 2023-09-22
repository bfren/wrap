// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Ok{T}"/> and the value is true.
	/// </summary>>
	/// <param name="this">Result object.</param>
	public static void AssertTrue(this Result<bool> @this) =>
		Assert.True(@this.AssertSome());
}
