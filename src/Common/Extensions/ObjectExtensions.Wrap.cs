// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class ObjectExtensions
{
	/// <inheritdoc cref="F.Wrap{TMonad, TValue}(TValue)"/>
	public static Monad<T> Wrap<T>(this T value) =>
		F.Wrap<Monad<T>, T>(value);
}
