// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Implementation using <see cref="ulong"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class ULongId<TId>() : Id<TId, ulong>(0UL), IULongId<TId>
	where TId : ULongId<TId>, new();
