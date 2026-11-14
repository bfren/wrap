// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Strongly typed ID with a <see cref="ulong"/> value.
/// </summary>
public interface IULongId : IId<ulong>;

/// <summary>
/// Strongly typed ID with a <see cref="ulong"/> value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IULongId<TId> : IULongId, IId<TId, ulong>
	where TId : class, IId<TId, ulong>, new()
{ }
