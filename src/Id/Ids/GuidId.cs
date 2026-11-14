// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Ids;

/// <summary>
/// Base record for strongly typed IDs with a <see cref="Guid"/> value; a new ID holds the default value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class GuidId<TId>() : Id<TId, Guid>(Guid.Empty), IGuidId<TId>
	where TId : GuidId<TId>, new();
