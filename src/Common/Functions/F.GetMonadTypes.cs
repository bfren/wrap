// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Get the implentation and value types if <paramref name="type"/> implements <see cref="IMonad{TMonad, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic type definition (e.g. <see cref="IMonad{TMonad, TValue}"/>).</param>
	/// <returns>Monad implementation and value types.</returns>
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
