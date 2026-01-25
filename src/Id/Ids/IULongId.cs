// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Generic ID using <see cref="ulong"/> as the Value type.
/// </summary>
public interface IULongId : IId<ulong>;

/// <summary>
/// Generid ID using <see cref="ulong"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IULongId<TId> : IULongId, IId<TId, ulong>
	where TId : class, IId<TId, ulong>, new()
{ }
