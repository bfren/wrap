// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Generic ID using <see cref="uint"/> as the Value type.
/// </summary>
public interface IUintId : IId<uint>;

/// <summary>
/// Generid ID using <see cref="uint"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IUintId<TId> : IUintId, IId<TId, uint>
	where TId : class, IId<TId, uint>, new()
{ }
