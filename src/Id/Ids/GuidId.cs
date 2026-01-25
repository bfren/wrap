// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Ids;

/// <summary>
/// Implementation using <see cref="Guid"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public abstract record class GuidId<TId>() : Id<TId, Guid>(Guid.Empty), IGuidId<TId>
	where TId : GuidId<TId>, new();
