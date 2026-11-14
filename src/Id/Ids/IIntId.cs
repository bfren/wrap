// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Ids;

/// <summary>
/// Strongly typed ID with a <see cref="int"/> value.
/// </summary>
public interface IIntId : IId<int>;

/// <summary>
/// Strongly typed ID with a <see cref="int"/> value.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
public interface IIntId<TId> : IIntId, IId<TId, int>
	where TId : class, IId<TId, int>, new()
{ }
