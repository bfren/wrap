// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Base record for strongly typed IDs with a <see cref="long"/> value; a new ID holds the default value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class LongId<TId>() : Id<TId, long>(0L), ILongId<TId>
	where TId : LongId<TId>, new();
