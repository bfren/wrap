// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="int"/> objects.
/// </summary>
public static class IntExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this int @this)
		where TId : IntId<TId>, IIntId, new() =>
		I.Wrap<TId, int>(@this);
}
