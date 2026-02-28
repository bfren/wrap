// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Common utility functions for the Wrap library.
/// </summary>
public static partial class F
{
	/// <summary>
	/// Check whether or not <typeparamref name="T"/> is a nullable value type.
	/// </summary>
	/// <typeparam name="T">Type to check.</typeparam>
	/// <param name="_">The object is unneeded.</param>
	/// <returns>True if <typeparamref name="T"/> is a nullable value type.</returns>
	public static bool IsNullableValueType<T>(T _) =>
		NullableHelper<T>.IsNullable;

	private static class NullableHelper<T>
	{
		public static readonly bool IsNullable =
			typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) is not null;
	}
}
