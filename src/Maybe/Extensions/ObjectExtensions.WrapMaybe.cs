// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="M.Wrap{T}(T)"/>
	public static Maybe<T> WrapMaybe<T>(this T value) =>
		M.Wrap(value);
}
