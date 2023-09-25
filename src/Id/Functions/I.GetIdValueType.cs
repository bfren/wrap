// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;

namespace Wrap;

public static partial class I
{
	/// <summary>
	/// Get the <see cref="Id{TId, TValue}"/> Value type if <paramref name="type"/>
	/// implements <see cref="Id{TId, TValue}"/>
	/// </summary>
	/// <param name="type">Type to check</param>
	public static Type? GetIdValueType(Type type)
	{
		// Strong IDs must implement IStrongId as a minimum
		if (!typeof(Id<,>).IsAssignableFrom(type))
		{
			return null;
		}

		// Get    .. all interfaces implemented by the type we are checking
		// If     .. the interface is a generic type (i.e. has generic type arguments)
		//        .. and the generic type definition is Id<,>
		// Select .. the second generic type argument - this is the Id<,> Value type
		var valueTypesQuery = from i in type.GetInterfaces()
							  where i.IsGenericType
							  && i.GetGenericTypeDefinition() == typeof(Id<,>)
							  select i.GenericTypeArguments[1];
		var valueTypes = valueTypesQuery.ToList();

		// If the count is not 1, this means the type doesn't implement Id<,>,
		// or it implements it multiple times, which is not supported
		if (valueTypes.Count != 1)
		{
			return null;
		}

		// Precisely one value type has been identified, so return it
		return valueTypes[0];
	}
}
