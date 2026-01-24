// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Data;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="LongId{TId}"/> TypeHandler.
/// </summary>
/// <typeparam name="T"><see cref="LongId{TId}"/> type.</typeparam>
public sealed class LongIdTypeHandler<T> : IdTypeHandler<T, long>
	where T : LongId<T>, ILongId, new()
{
	/// <summary>
	/// Parse value and create new <see cref="LongId{TId}"/>.
	/// </summary>
	/// <param name="value"><see cref="LongId{TId}"/> Value.</param>
	/// <returns><see cref="LongId{TId}"/> with parsed value.</returns>
	public override T Parse(object value) =>
		new() { Value = M.ParseInt64(value?.ToString()).Unwrap(() => 0L) };

	/// <summary>
	/// Set ID value.
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="value"><see cref="LongId{TId}"/> value.</param>
	public override void SetValue(IDbDataParameter parameter, T? value)
	{
		// Set DbType according to the ID value type rather than the StrongId type
		parameter.DbType = DbType.Int64;

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
