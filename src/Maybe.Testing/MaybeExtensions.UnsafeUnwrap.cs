// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> is a <see cref="Some{T}"/> and get the value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is particularly used to get values simply during the Arrange section of a unit test.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Maybe object.</param>
	public static T UnsafeUnwrap<T>(this Maybe<T> @this) =>
		@this.Unwrap(() => throw new Exceptions.UnsafeUnwrapException());
}
