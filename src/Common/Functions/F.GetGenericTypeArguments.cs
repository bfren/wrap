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
	/// Get the generic type arguments of every interface on <paramref name="type"/> built from <paramref name="genericType"/>.
	/// </summary>
	/// <remarks>
	/// Results are cached for each pair of types.
	/// </remarks>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic interface type definition, for example <see cref="IMonad{TMonad, TValue}"/>.</param>
	/// <returns>The type arguments of each matching interface, in order, or an empty array if <paramref name="type"/> does not implement <paramref name="genericType"/>.</returns>
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
