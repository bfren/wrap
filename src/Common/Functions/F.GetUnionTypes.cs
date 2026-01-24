// Wrap: .NET monads for functional style.
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
		//        .. and the generic type definition is IUnion<,>
		// Select .. all generic type arguments: the Union implementation type and value type
		[..
			from i in type.GetInterfaces()
			where i.IsGenericType && i.GetGenericTypeDefinition() == genericType
			from a in i.GenericTypeArguments
			select a
		];

	/// <summary>
	/// Get the implentation and value types if <paramref name="type"/> implements <see cref="IUnion{TUnion, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic type definition (e.g. <see cref="IUnion{TUnion, TValue}"/>).</param>
	/// <returns>Union implementation and value types.</returns>
	public static (Type? unionType, Type? valueType) GetUnionTypes(Type type, Type genericType)
	{
		// IDs must implement IUnion as a minimum
		if (!typeof(IUnion).IsAssignableFrom(type))
		{
			return (null, null);
		}

		// Get generic type arguments
		var types = GetGenericTypeArguments(type, genericType);

		// If the count is not 2, this means the type doesn't implement IUnion<TUnion, TValue>,
		// or it implements it multiple times, which is not supported
		if (types.Count != 2)
		{
			return (null, null);
		}

		// Return both types
		return (types[0], types[1]);
	}
}
