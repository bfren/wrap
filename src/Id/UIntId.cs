// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Implementation using <see cref="uint"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class UIntId<TId>() : Id<TId, uint>(0u), IUintId
	where TId : UIntId<TId>, IUintId, new();
