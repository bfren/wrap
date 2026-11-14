// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Data;
using Wrap.Ids;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="GuidId{TId}"/> TypeHandler.
/// </summary>
/// <typeparam name="T"><see cref="GuidId{TId}"/> type.</typeparam>
public sealed class GuidIdTypeHandler<T> : IdTypeHandler<T, Guid>
	where T : GuidId<T>, new()
{
	/// <summary>
	/// Parse value and create new <see cref="GuidId{TId}"/>.
	/// </summary>
	/// <param name="value"><see cref="GuidId{TId}"/> Value.</param>
	/// <returns><see cref="GuidId{TId}"/> with parsed value.</returns>
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
	/// Set ID value.
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="value"><see cref="GuidId{TId}"/> value.</param>
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
