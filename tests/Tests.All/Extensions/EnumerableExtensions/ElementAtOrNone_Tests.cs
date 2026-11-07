// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class ElementAtOrNone_Tests
{
	public class With_Empty_List
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var list = Array.Empty<Guid>();

			// Act
			var result = list.ElementAtOrNone(Rnd.Int);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Values
	{
		[Fact]
		public void Returns_Correct_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var list = new[] { Rnd.Guid, Rnd.Guid, value, Rnd.Guid };

			// Act
			var result = list.ElementAtOrNone(2);

			// Assert
			result.AssertSome(value);
		}
	}

	public class With_Negative_Index
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var list = new[] { Rnd.Guid, Rnd.Guid };

			// Act
			var result = list.ElementAtOrNone(-1);

			// Assert
			result.AssertNone();
		}
	}

	public class With_Endless_Sequence
	{
		[Fact]
		public void Stops_At_Index()
		{
			// Arrange
			static IEnumerable<int> endless() { var i = 0; while (true) { yield return i++; } }

			// Act
			var result = endless().ElementAtOrNone(3);

			// Assert
			result.AssertSome(3);
		}
	}
}
