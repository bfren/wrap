// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Check whether or not <typeparamref name="T"/> is a nullable value type.
	/// </summary>
	/// <typeparam name="T">Type to check.</typeparam>
	/// <param name="_">Value used only to infer <typeparamref name="T"/>; it is not read.</param>
	/// <returns><see langword="true"/> if <typeparamref name="T"/> is a nullable value type (<see cref="Nullable{T}"/>), otherwise <see langword="false"/>.</returns>
	public static bool IsNullableValueType<T>(T _) =>
		NullableHelper<T>.IsNullable;

	private static class NullableHelper<T>
	{
		public static readonly bool IsNullable =
			typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) is not null;
	}
}
