// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Wrap;

public static partial class F
{
	internal static readonly ConcurrentDictionary<(Type type, Type genericType), Type[]> GenericTypeCache =
		new();

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
		GenericTypeCache.GetOrAdd((type, genericType), static key =>
		[..
			from i in key.type.GetInterfaces()
			where i.IsGenericType && i.GetGenericTypeDefinition() == key.genericType
			from a in i.GenericTypeArguments
			select a
		]);
}
