// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="uint"/> objects.
/// </summary>
public static class UIntExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this uint value)
		where TId : UIntId, new() =>
		I.Wrap<TId, uint>(value);
}
