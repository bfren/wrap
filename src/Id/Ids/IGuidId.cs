// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Ids;

/// <summary>
/// Generic ID using <see cref="Guid"/> as the Value type.
/// </summary>
public interface IGuidId : IId<Guid>;

/// <summary>
/// Generic ID using <see cref="Guid"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IGuidId<TId> : IGuidId, IId<TId, Guid>
	where TId : class, IId<TId, Guid>, new()
{ }
