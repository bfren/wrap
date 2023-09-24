// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class ObjectExtensions
{
	public static Result<T> WrapResult<T>(this T obj) =>
		R.Wrap(obj);
}
