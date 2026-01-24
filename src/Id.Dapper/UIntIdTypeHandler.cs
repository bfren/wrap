// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Data;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="UIntId{TId}"/> TypeHandler.
/// </summary>
/// <typeparam name="T"><see cref="UIntId{TId}"/> type.</typeparam>
public sealed class UIntIdTypeHandler<T> : IdTypeHandler<T, uint>
	where T : UIntId<T>, IUintId, new()
{
	/// <summary>
	/// Parse value and create new <see cref="UIntId{TId}"/>.
	/// </summary>
	/// <param name="value"><see cref="UIntId{TId}"/> Value.</param>
	/// <returns><see cref="UIntId{TId}"/> with parsed value.</returns>
	public override T Parse(object value) =>
		new() { Value = M.ParseUInt32(value?.ToString()).Unwrap(() => 0U) };

	/// <summary>
	/// Set ID value.
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="value"><see cref="UIntId{TId}"/> value.</param>
	public override void SetValue(IDbDataParameter parameter, T? value)
	{
		// Set DbType according to the ID value type rather than the StrongId type
		parameter.DbType = DbType.UInt32;

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
