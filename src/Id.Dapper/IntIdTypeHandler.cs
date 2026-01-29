// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Data;
using Wrap.Ids;

namespace Wrap.Dapper;

/// <summary>
/// <see cref="IntId{TId}"/> TypeHandler.
/// </summary>
/// <typeparam name="T"><see cref="IntId{TId}"/> type.</typeparam>
public sealed class IntIdTypeHandler<T> : IdTypeHandler<T, int>
	where T : IntId<T>, new()
{
	/// <summary>
	/// Parse value and create new <see cref="IntId{TId}"/>.
	/// </summary>
	/// <param name="value"><see cref="IntId{TId}"/> Value.</param>
	/// <returns><see cref="IntId{TId}"/> with parsed value.</returns>
	public override T Parse(object value) =>
		new() { Value = M.ParseInt32(value?.ToString()).Unwrap(() => 0) };

	/// <summary>
	/// Set ID value.
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="value"><see cref="IntId{TId}"/> value.</param>
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
				0
		};
	}
}
