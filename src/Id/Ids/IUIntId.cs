// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Strongly typed ID with a <see cref="uint"/> value.
/// </summary>
public interface IUIntId : IId<uint>;

/// <summary>
/// Strongly typed ID with a <see cref="uint"/> value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IUIntId<TId> : IUIntId, IId<TId, uint>
	where TId : class, IId<TId, uint>, new()
{ }
