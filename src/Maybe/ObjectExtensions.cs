// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public static class ObjectExtensions
{
	public static Maybe<T> ToMaybe<T>(this T obj) =>
		obj;
}
