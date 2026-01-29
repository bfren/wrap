// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Wrap_Tests
{
	public class With_Null
	{
		public class Reference_Type
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange

				// Act
				var result = R.Wrap<string>(null!);

				// Assert
				result.AssertFailure(
					"Null value of type '{Type}' - try using Maybe<T> to wrap null values safely.",
					nameof(String)
				);
			}
		}

		public class Nullable_Reference_Type
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange

				// Act
				var result = R.Wrap<string?>(null);

				// Assert
				result.AssertFailure(
					"Null value of type '{Type}' - try using Maybe<T> to wrap null values safely.",
					nameof(String)
				);
			}
		}

		public class Nullable_Value_Type
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange

				// Act
				var result = R.Wrap<int?>(null);

				// Assert
				result.AssertFailure(
					"Null value of type '{Type}' - try using Maybe<T> to wrap null values safely.",
					typeof(int?).Name
				);
			}
		}
	}

	public class With_Value
	{
		[Fact]
		public void Returns_Ok_With_Value()
		{
			// Arrange
			var value = Rnd.UIntPtr;

			// Act
			var result = R.Wrap(value);

			// Assert
			result.AssertOk(value);
		}
	}
}
