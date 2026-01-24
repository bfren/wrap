// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;

namespace Wrap;

public static partial class I
{
	/// <summary>
	/// Get all base types for the specified Type.
	/// </summary>
	/// <param name="this">Type to check.</param>
	/// <returns>List of base types.</returns>
	public static IEnumerable<Type> GetBaseTypes(this Type @this)
	{
		var current = @this.BaseType;
		while (current != null)
		{
			yield return current;
			current = current.BaseType;
		}
	}
}
