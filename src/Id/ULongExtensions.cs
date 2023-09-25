// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="ulong"/> objects.
/// </summary>
public static class ULongExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this ulong value)
		where TId : ULongId<TId>, new() =>
		I.Wrap<TId, ulong>(value);
}
