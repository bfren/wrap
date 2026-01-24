// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Data;

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
		new() { Value = M.ParseGuid(value?.ToString()).Unwrap(() => Guid.Empty) };

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
				0
		};
	}
}
