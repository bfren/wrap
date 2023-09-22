// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static class ObjectExtensions
{
	public static Result<T> ToResult<T>(this T obj) =>
		obj;
}
