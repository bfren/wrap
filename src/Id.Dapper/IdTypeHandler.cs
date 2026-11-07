// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
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
{
	/// <summary>
	/// Returns the default ID value for null, empty or non-numeric input, but throws when
	/// <paramref name="value"/> is a number that does not fit in <typeparamref name="TIdValue"/>
	/// (e.g. a bigint mapped to an int ID) - otherwise real IDs would silently become 0.
	/// </summary>
	/// <param name="value">Database value that could not be parsed.</param>
	/// <exception cref="OverflowException"></exception>
	protected static TIdValue DefaultOrThrow(object? value) =>
		value switch
		{
			sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal =>
				throw new OverflowException($"Value '{value}' cannot be stored in {typeof(TIdValue).Name} for {typeof(TId).Name}."),

			_ =>
				default
		};
}
