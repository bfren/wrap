// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Maybe_Tests;

public class GetHashCode_Tests
{
	public class Some
	{
		public class Same_Type
		{
			public class Same_Value
			{
				[Fact]
				public void Returns_Same_HashCode()
				{
					// Arrange
					var value = Rnd.Int;
					var m0 = M.Wrap(value);
					var m1 = M.Wrap(value);

					// Act
					var r0 = m0.GetHashCode();
					var r1 = m1.GetHashCode();

					// Assert
					Assert.Equal(r0, r1);
				}
			}

			public class Different_Value
			{
				public void Returns_Different_HashCode()
				{
					// Arrange
					var m0 = M.Wrap(Rnd.Int);
					var m1 = M.Wrap(Rnd.Int);

					// Act
					var r0 = m0.GetHashCode();
					var r1 = m1.GetHashCode();

					// Assert
					Assert.NotEqual(r0, r1);
				}
			}
		}
	}

	public class None
	{
		[Fact]
		public void Returns_None_HashCode()
		{
			// Arrange
			var maybe = NoneGen.Create<int>();

			// Act
			var result = maybe.GetHashCode();

			// Assert
			Assert.Equal(M.None.GetHashCode(), result);
		}
	}

	public class Invalid_Type
	{
		[Fact]
		public void Throws_InvalidMaybeTypeException()
		{
			// Arrange
			var maybe = new InvalidMaybe<DateTime>();

			// Act
			var result = Record.Exception(() => maybe.GetHashCode());

			// Assert
			_ = Assert.IsType<InvalidMaybeTypeException>(result);
		}
	}
}
