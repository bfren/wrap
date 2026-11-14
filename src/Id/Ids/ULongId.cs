// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Base record for strongly typed IDs with a <see cref="ulong"/> value; a new ID holds the default value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class ULongId<TId>() : Id<TId, ulong>(0UL), IULongId<TId>
	where TId : ULongId<TId>, new();
