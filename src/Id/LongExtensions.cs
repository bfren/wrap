// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="long"/> objects.
/// </summary>
public static class LongExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this long value)
		where TId : LongId<TId>, new() =>
		I.Wrap<TId, long>(value);
}
