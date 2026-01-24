// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Data;

namespace Abstracts;

public abstract class SetValue_Tests<TId, TIdValue, TIdTypeHandler>
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
	where TIdTypeHandler : IdTypeHandler<TId, TIdValue>, new()
{
	public abstract void Test00_Sets_Parameter__With_Correct_Type_And_Value();

	protected static void Test00(TId id, DbType expectedType)
	{
		// Arrange
		var handler = new TIdTypeHandler();
		var parameter = Substitute.For<IDbDataParameter>();

		// Act
		handler.SetValue(parameter, id);

		// Assert
		Assert.Equal(expectedType, parameter.DbType);
		Assert.Equal(id.Value, parameter.Value);
	}
}
