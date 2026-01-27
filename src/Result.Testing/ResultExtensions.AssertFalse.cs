// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Ok{T}"/> and the value is false.
	/// </summary>
	/// <param name="this">Result object.</param>
	public static void AssertFalse(this Result<bool> @this) =>
		Assert.False(@this.AssertOk());
}
