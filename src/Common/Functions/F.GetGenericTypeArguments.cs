// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Get generic type arguments if <paramref name="type"/> implements <paramref name="genericType"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Implemented generic type.</param>
	/// <returns></returns>
	internal static Type[] GetGenericTypeArguments(Type type, Type genericType) =>
		// Get    .. all interfaces implemented by the type we are checking
		// If     .. the interface is a generic type (i.e. has generic type arguments)
		//        .. and the generic type definition is genericType
		// Select .. all generic type arguments: the Either implementation type and left / right value types
		[..
			from i in type.GetInterfaces()
			where i.IsGenericType && i.GetGenericTypeDefinition() == genericType
			from a in i.GenericTypeArguments
			select a
		];
}
