// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Get the value type if <paramref name="type"/> implements <see cref="IUnion{TUnion, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic type definition (e.g. <see cref="IUnion{TUnion, TValue}"/>).</param>
	/// <returns>Union value type.</returns>
	public static Type? GetUnionValueType(Type type, Type genericType) =>
		GetUnionTypes(type, genericType).valueType;
}
