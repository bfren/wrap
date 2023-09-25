// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Extension methods for <see cref="Guid"/> objects.
/// </summary>
public static class GuidExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this Guid value)
		where TId : GuidId, new() =>
		I.Wrap<TId, Guid>(value);
}
