// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="F.Wrap{TUnion, TValue}(TValue)"/>
	public static Union<T> Wrap<T>(this T value) =>
		F.Wrap<Union<T>, T>(value);
}
