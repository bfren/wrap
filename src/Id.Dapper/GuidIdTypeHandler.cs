// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Data;
using Wrap.Ids;

namespace Wrap.Dapper;

/// <summary>
/// Dapper type handler that reads and writes <see cref="GuidId{TId}"/> values.
/// </summary>
/// <typeparam name="T"><see cref="GuidId{TId}"/> implementation type.</typeparam>
public sealed class GuidIdTypeHandler<T> : IdTypeHandler<T, Guid>
	where T : GuidId<T>, new()
{
	/// <summary>
	/// Parse a database value and create a new <typeparamref name="T"/>.
	/// </summary>
	/// <remarks>
	/// A 16-byte array (returned by some providers, for example Microsoft.Data.Sqlite) is read in
	/// <see cref="Guid.ToByteArray()"/> order; anything else is parsed from its string form.
	/// </remarks>
	/// <param name="value">Database value.</param>
	/// <returns>A new <typeparamref name="T"/> holding the parsed value, or the default value if <paramref name="value"/> is <see langword="null"/> or cannot be parsed.</returns>
	/// <exception cref="OverflowException"><paramref name="value"/> is a number that cannot be stored in the ID value type.</exception>
	public override T Parse(object value) =>
		new()
		{
			Value = value switch
			{
				// some providers (e.g. Microsoft.Data.Sqlite) return Guid columns as raw bytes, which have no string form to parse
				// shortcut: assumes Guid.ToByteArray() byte order - RFC 4122-ordered bytes (MySQL UUID_TO_BIN, Oracle SYS_GUID) read as a different Guid
				byte[] { Length: 16 } bytes =>
					new Guid(bytes),

				_ =>
					M.ParseGuid(value?.ToString()) switch
					{
						Some<Guid> x =>
							x.Value,

						None =>
							DefaultOrThrow(value)
					}
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
		parameter.DbType = DbType.Guid;

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
