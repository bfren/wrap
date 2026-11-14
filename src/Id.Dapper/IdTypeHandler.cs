// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Dapper;

namespace Wrap.Dapper;

/// <summary>
/// Base class for Dapper type handlers that read and write <see cref="Id{TId, TValue}"/> values.
/// </summary>
/// <typeparam name="TId">ID implementation type.</typeparam>
/// <typeparam name="TIdValue">ID value type.</typeparam>
public abstract class IdTypeHandler<TId, TIdValue> : SqlMapper.TypeHandler<TId>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{
	/// <summary>
	/// Return the default ID value for a database value that could not be parsed, unless it is a number.
	/// </summary>
	/// <remarks>
	/// A number that could not be parsed does not fit in <typeparamref name="TIdValue"/> (for example a bigint mapped
	/// to an int ID), so it throws rather than letting real IDs silently become 0.
	/// </remarks>
	/// <param name="value">Database value that could not be parsed.</param>
	/// <returns>The default value of <typeparamref name="TIdValue"/>, for <see langword="null"/>, empty or non-numeric input.</returns>
	/// <exception cref="OverflowException"><paramref name="value"/> is a number.</exception>
	protected static TIdValue DefaultOrThrow(object? value) =>
		value switch
		{
			sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal =>
				throw new OverflowException($"Value '{value}' cannot be stored in {typeof(TIdValue).Name} for {typeof(TId).Name}."),

			_ =>
				default
		};
}
