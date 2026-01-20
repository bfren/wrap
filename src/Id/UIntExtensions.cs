// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="uint"/> objects.
/// </summary>
public static class UIntExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this uint @this)
		where TId : UIntId<TId>, IUintId, new() =>
		I.Wrap<TId, uint>(@this);
}
