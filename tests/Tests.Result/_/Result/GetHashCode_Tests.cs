// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Result_Tests;

public class GetHashCode_Tests
{
	public class Ok
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
					var v0 = R.Wrap(value);
					var v1 = R.Wrap(value);

					// Act
					var r0 = v0.GetHashCode();
					var r1 = v1.GetHashCode();

					// Assert
					Assert.Equal(r0, r1);
				}
			}

			public class Different_Value
			{
				public void Returns_Different_HashCode()
				{
					// Arrange
					var v0 = R.Wrap(Rnd.Int);
					var v1 = R.Wrap(Rnd.Int);

					// Act
					var r0 = v0.GetHashCode();
					var r1 = v1.GetHashCode();

					// Assert
					Assert.NotEqual(r0, r1);
				}
			}
		}
	}

	public class Failure
	{
		public class Same_Value
		{
			[Fact]
			public void Returns_Same_HashCode()
			{
				// Arrange
				var value = FailGen.Value;
				var v0 = FailGen.Create<string>(value);
				var v1 = FailGen.Create<string>(value);

				// Act
				var r0 = v0.GetHashCode();
				var r1 = v1.GetHashCode();

				// Assert
				Assert.Equal(r0, r1);
			}
		}

		public class Different_Value
		{
			public void Returns_Different_HashCode()
			{
				// Arrange
				var v0 = FailGen.Create<string>();
				var v1 = FailGen.Create<string>();

				// Act
				var r0 = v0.GetHashCode();
				var r1 = v1.GetHashCode();

				// Assert
				Assert.NotEqual(r0, r1);
			}
		}
	}

	public class Invalid_Type
	{
		[Fact]
		public void Throws_InvalidResultTypeException()
		{
			// Arrange
			var maybe = new InvalidResult<DateTime>();

			// Act
			var result = Record.Exception(() => maybe.GetHashCode());

			// Assert
			_ = Assert.IsType<InvalidResultTypeException>(result);
		}
	}
}
