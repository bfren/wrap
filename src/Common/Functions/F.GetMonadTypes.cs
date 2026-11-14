// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Get the implementation and value types if <paramref name="type"/> implements <paramref name="genericType"/> once.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic interface type definition with two type arguments, for example <see cref="IMonad{TMonad, TValue}"/>.</param>
	/// <returns>
	/// The monad implementation and value types, or two <see langword="null"/> values if <paramref name="type"/>
	/// does not implement <paramref name="genericType"/> or implements it more than once.
	/// </returns>
	public static (Type? monadType, Type? valueType) GetMonadTypes(Type type, Type genericType)
	{
		// Get generic type arguments
		var types = GetGenericTypeArguments(type, genericType);

		// If the count is not 2, this means the type doesn't implement IMonad<TMonad, TValue>,
		// or it implements it multiple times, which is not supported
		if (types.Length != 2)
		{
			return (null, null);
		}

		// Return both types
		return (types[0], types[1]);
	}
}
