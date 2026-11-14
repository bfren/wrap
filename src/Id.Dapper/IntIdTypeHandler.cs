// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Data;
using Wrap.Ids;

namespace Wrap.Dapper;

/// <summary>
/// Dapper type handler that reads and writes <see cref="IntId{TId}"/> values.
/// </summary>
/// <typeparam name="T"><see cref="IntId{TId}"/> implementation type.</typeparam>
public sealed class IntIdTypeHandler<T> : IdTypeHandler<T, int>
	where T : IntId<T>, new()
{
	/// <summary>
	/// Parse a database value and create a new <typeparamref name="T"/>.
	/// </summary>
	/// <param name="value">Database value.</param>
	/// <returns>A new <typeparamref name="T"/> holding the parsed value, or the default value if <paramref name="value"/> is <see langword="null"/> or cannot be parsed.</returns>
	/// <exception cref="OverflowException"><paramref name="value"/> is a number that cannot be stored in the ID value type.</exception>
	public override T Parse(object value) =>
		new()
		{
			Value = M.ParseInt32(value?.ToString()) switch
			{
				Some<int> x =>
					x.Value,

				None =>
					DefaultOrThrow(value)
			}
		};

	/// <summary>
	/// Set a parameter to the ID value, with the matching <see cref="DbType"/>.
	/// </summary>
	/// <param name="parameter">Parameter to set.</param>
	/// <param name="value">ID to write, or <see langword="null"/> to write <see cref="DBNull.Value"/>.</param>
	public override void SetValue(IDbDataParameter parameter, T? value)
	{
		// Set DbType according to the ID value type rather than the StrongId type
		parameter.DbType = DbType.Int32;

		// If the value is null, use default ID value
		parameter.Value = value switch
		{
			T id =>
				id.Value,

			_ =>
				DBNull.Value
		};
	}
}
