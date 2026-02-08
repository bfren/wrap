// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Abstracts;

public abstract class Value_Tests
{
	private object Get<T>(T id)
		where T : IMonad =>
		id.Value;

	private TId Set<TId, TValue>(TValue value)
		where TId : IMonad<TValue>, new() =>
		new() { Value = value };

	public abstract void Test00_Generic_Get__With_Value__Returns_Value();

	protected void Test00<TId>(TId input)
		where TId : IMonad, new()
	{
		// Arrange

		// Act
		var result = Get(input);

		// Assert
		Assert.Equal(input.Value, result);
	}

	public abstract void Test01_Generic_Set__Receives_Correct_Type__Uses_Value();

	public void Test01<TId, TValue>(TValue input)
		where TId : IMonad<TValue>, new()
	{
		// Arrange

		// Act
		var result = Set<TId, TValue>(input);

		// Assert
		Assert.Equal(input, result.Value);
	}
}
