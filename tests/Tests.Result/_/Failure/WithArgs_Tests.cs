// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class WithArgs_Tests
{
	public class With_Null
	{
		[Fact]
		public void Does_Nothing()
		{
			// Arrange
			var failure = R.Fail(Rnd.Str, Rnd.Int, Rnd.Guid);

			// Act
			var result = failure.WithArgs(null!);

			// Assert
			Assert.Equal(failure.Args, result.Args);
		}
	}

	public class With_Empty_Args
	{
		[Fact]
		public void Unsets_Args()
		{
			// Arrange
			var failure = R.Fail(Rnd.Str, Rnd.Int, Rnd.Guid);

			// Act
			var result = failure.WithArgs([]);

			// Assert
			Assert.Empty(result.Args);
		}
	}

	public class With_Args
	{
		[Fact]
		public void Adds_Args_To_Failure()
		{
			// Arrange
			var failure = R.Fail(Rnd.Str, Rnd.Int, Rnd.Guid);
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = failure.WithArgs(args);

			// Assert
			Assert.Equal(args, result.Args);
		}
	}
}
