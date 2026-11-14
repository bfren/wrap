// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Ids;

/// <summary>
/// Strongly typed ID with a <see cref="Guid"/> value.
/// </summary>
public interface IGuidId : IId<Guid>;

/// <summary>
/// Strongly typed ID with a <see cref="Guid"/> value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IGuidId<TId> : IGuidId, IId<TId, Guid>
	where TId : class, IId<TId, Guid>, new()
{ }
