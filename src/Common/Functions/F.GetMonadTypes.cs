// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrap;

public static partial class F
{
	private static List<Type> GetGenericTypeArguments(Type type, Type genericType) =>
		// Get    .. all interfaces implemented by the type we are checking
		// If     .. the interface is a generic type (i.e. has generic type arguments)
		//        .. and the generic type definition is IMonad<,>
		// Select .. all generic type arguments: the Monad implementation type and value type
		[..
			from i in type.GetInterfaces()
			where i.IsGenericType && i.GetGenericTypeDefinition() == genericType
			from a in i.GenericTypeArguments
			select a
		];

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
		if (types.Count != 2)
		{
			return (null, null);
		}

		// Return both types
		return (types[0], types[1]);
	}
}
