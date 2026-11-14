// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Strongly typed ID with a <see cref="long"/> value.
/// </summary>
public interface ILongId : IId<long>;

/// <summary>
/// Strongly typed ID with a <see cref="long"/> value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface ILongId<TId> : ILongId, IId<TId, long>
	where TId : class, IId<TId, long>, new()
{ }
