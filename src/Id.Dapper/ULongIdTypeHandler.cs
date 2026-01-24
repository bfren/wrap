// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Data;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="ULongId{TId}"/> TypeHandler.
/// </summary>
/// <typeparam name="T"><see cref="ULongId{TId}"/> type.</typeparam>
public sealed class ULongIdTypeHandler<T> : IdTypeHandler<T, ulong>
	where T : ULongId<T>, new()
{
	/// <summary>
	/// Parse value and create new <see cref="ULongId{TId}"/>.
	/// </summary>
	/// <param name="value"><see cref="ULongId{TId}"/> Value.</param>
	/// <returns><see cref="ULongId{TId}"/> with parsed value.</returns>
	public override T Parse(object value) =>
		new() { Value = M.ParseUInt64(value?.ToString()).Unwrap(() => 0UL) };

	/// <summary>
	/// Set ID value.
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="value"><see cref="ULongId{TId}"/> value.</param>
	public override void SetValue(IDbDataParameter parameter, T? value)
	{
		// Set DbType according to the ID value type rather than the StrongId type
		parameter.DbType = DbType.UInt64;

		// If the value is null, use default ID value
		parameter.Value = value switch
		{
			T id =>
				id.Value,

			_ =>
				0
		};
	}
}
