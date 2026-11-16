// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Linq.MaybeToMaybeExtensions_Tests;

public class Select_Tests
{
	// query syntax, not method calls: a second Select for Maybe<T> in Wrap.Linq makes these fail to compile (CS1940)
	[Fact]
	public void Query_With_Select__Returns_Some_With_Selected_Value()
	{
		// Arrange
		var value = Rnd.Int;

		// Act
		var result = from x in M.Wrap(value)
					 select x + 1;

		// Assert
		result.AssertSome(value + 1);
	}

	[Fact]
	public void Query_With_Let__Returns_Some_With_Selected_Value()
	{
		// Arrange
		var value = Rnd.Int;

		// Act
		var result = from x in M.Wrap(value)
					 let y = x + 1
					 select x + y;

		// Assert
		result.AssertSome(value + value + 1);
	}

	[Fact]
	public void Query_With_None__Returns_None()
	{
		// Arrange
		Maybe<int> input = M.None;

		// Act
		var result = from x in input
					 select x + 1;

		// Assert
		result.AssertNone();
	}
}
