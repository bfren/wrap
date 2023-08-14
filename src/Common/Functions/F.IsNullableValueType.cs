// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public static partial class F
{
	/// <summary>
	/// Check whether or not <typeparamref name="T"/> is a nullable value type.
	/// </summary>
	/// <typeparam name="T">Type to check.</typeparam>
	/// <param name="_">The object is unneeded.</param>
	/// <returns>True if <typeparamref name="T"/> is a nullable value type</returns>
	public static bool IsNullableValueType<T>(T _)
	{
		var t = typeof(T);
		if (t.IsValueType && Nullable.GetUnderlyingType(t) is not null)
		{
			return true;
		}

		return false;
	}
}
