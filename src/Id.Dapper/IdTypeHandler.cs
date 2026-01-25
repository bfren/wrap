// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Dapper;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="Id{TId, TValue}"/> TypeHandler.
/// </summary>
/// <typeparam name="TId">Implementation type.</typeparam>
/// <typeparam name="TIdValue">ID value type.</typeparam>
public abstract class IdTypeHandler<TId, TIdValue> : SqlMapper.TypeHandler<TId>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{ }
