// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Generic ID using <see cref="long"/> as the Value type.
/// </summary>
public interface ILongId : IId<long>;

/// <summary>
/// Generid ID using <see cref="long"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface ILongId<TId> : ILongId, IId<TId, long>
	where TId : class, IId<TId, long>, new()
{ }
