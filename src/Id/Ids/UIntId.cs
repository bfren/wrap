// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Base record for strongly typed IDs with a <see cref="uint"/> value; a new ID holds the default value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class UIntId<TId>() : Id<TId, uint>(0u), IUintId<TId>
	where TId : UIntId<TId>, new();
