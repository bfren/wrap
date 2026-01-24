// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrap;

public static partial class I
{
	private static List<Type> GetGenericTypeArguments(Type type) =>
		// Get    .. all interfaces implemented by the type we are checking
		// If     .. the interface is a generic type (i.e. has generic type arguments)
		//        .. and the generic type definition is IId<,>
		// Select .. all generic type arguments - this is the Id implementation type and Id value type
		[..
			from i in type.GetInterfaces()
			where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IId<,>)
			from a in i.GenericTypeArguments
			select a
		];

	/// <summary>
	/// Get the <see cref="Id{TId, TValue}"/> Value type if <paramref name="type"/>
	/// implements <see cref="IId{TId, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	public static Type? GetIdValueType(Type type) =>
		GetIdTypes(type).idValueType;

	/// <summary>
	/// Get the <see cref="Id{TId, TValue}"/> Implentation and Value types if <paramref name="type"/>
	/// implements <see cref="IId{TId, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <returns>ID implementation type and ID Value type.</returns>
	public static (Type? idType, Type? idValueType) GetIdTypes(Type type)
	{
		// IDs must implement IUnion as a minimum
		if (!typeof(IUnion).IsAssignableFrom(type))
		{
			return (null, null);
		}

		// Get generic type arguments
		var types = GetGenericTypeArguments(type);

		// If the count is not 2, this means the type doesn't implement IId<TId, TValue>,
		// or it implements it multiple times, which is not supported
		if (types.Count != 2)
		{
			return (null, null);
		}

		// Return both types
		return (types[0], types[1]);
	}
}
