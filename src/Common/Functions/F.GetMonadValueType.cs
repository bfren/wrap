// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public static partial class F
{
	/// <summary>
	/// Get the value type if <paramref name="type"/> implements <see cref="IMonad{TMonad, TValue}"/>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <param name="genericType">Generic type definition (e.g. <see cref="IMonad{TMonad, TValue}"/>).</param>
	/// <returns>Monad value type.</returns>
	public static Type? GetMonadValueType(Type type, Type genericType) =>
		GetMonadTypes(type, genericType).valueType;
}
