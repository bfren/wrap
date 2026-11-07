// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class FirstOrNone_Tests
{
	public class With_Empty_List
	{
		[Fact]
		public void Returns_None()
		{
			// Arrange
			var list = Array.Empty<Guid>();

			// Act
			var result = list.FirstOrNone();

			// Assert
			result.AssertNone();
		}
	}

	public class With_Values
	{
		[Fact]
		public void Returns_First_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var list = new[] { value, Rnd.Guid, Rnd.Guid, Rnd.Guid };

			// Act
			var result = list.FirstOrNone();

			// Assert
			result.AssertSome(value);
		}
	}

	public class With_Endless_Sequence
	{
		[Fact]
		public void Stops_At_First_Value()
		{
			// Arrange
			static IEnumerable<int> endless()
			{
				var i = 0;
				while (true)
				{
					yield return i++;
				}
			}

			// Act
			var result = endless().FirstOrNone();

			// Assert
			result.AssertSome(0);
		}
	}
}
