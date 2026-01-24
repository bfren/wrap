// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="long"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class LongId<TId>() : Id<TId, long>(0L), ILongId<TId>
	where TId : LongId<TId>, new();
