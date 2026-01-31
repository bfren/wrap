// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Result_Tests;

public class ToString_Tests
{
	public class When_Failure
	{
		[Fact]
		public void Returns_Failure_ToString()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Date;
			var f0 = R.Fail<int>("Failure with values {A} and {B}.", v0, v1);
			var f1 = R.Fail<int>("Failure with values {0} and {1}.", v0, v1);
			var expected = string.Format(F.DefaultCulture, "Failure with values {0} and {1}.", v0, v1);

			// Act
			var r0 = f0.ToString();
			var r1 = f1.ToString();

			// Assert
			Assert.Equal(expected, r0);
			Assert.Equal(expected, r1);
		}
	}

	public class When_Ok
	{
		public class When_Value_ToString_Returns_Null
		{
			[Fact]
			public void Returns_Ok_With_Type()
			{
				// Arrange
				var wrapped = R.Wrap(new Test());
				var expected = $"OK: {nameof(Test)}";

				// Act
				var result = wrapped.ToString();

				// Assert
				Assert.Equal(expected, result);
			}
		}

		[Fact]
		public void Returns_Ok_Value()
		{
			// Arrange
			var value = Rnd.Guid;
			var wrapped = R.Wrap(value);

			// Act
			var result = wrapped.ToString();

			// Assert
			Assert.Equal(value.ToString(), result);
		}
	}

	public class Test
	{
		public override string ToString() =>
			null!;
	}
}
