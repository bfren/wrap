// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> is a <see cref="Ok{T}"/> and get the value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is particularly used to get values simply during the Arrange section of a unit test.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="this">Result object.</param>
	public static T UnsafeUnwrap<T>(this Result<T> @this) =>
		@this.Unwrap(_ => throw new Exceptions.UnsafeUnwrapException());
}
