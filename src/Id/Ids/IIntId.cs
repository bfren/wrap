// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Generic ID using <see cref="int"/> as the Value type.
/// </summary>
public interface IIntId : IId<int>;

/// <summary>
/// Generid ID using <see cref="int"/> as the Value type.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IIntId<TId> : IIntId, IId<TId, int>
	where TId : class, IId<TId, int>, new()
{ }
