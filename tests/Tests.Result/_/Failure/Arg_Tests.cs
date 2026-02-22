// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Arg_Tests
{
	public class With_Null
	{
		[Fact]
		public void Does_Nothing()
		{
			// Arrange
			var failure = FailGen.Create();

			// Act
			var result = failure.Arg(null!);

			// Assert
			Assert.Equal(failure.Value.Args, result.Value.Args);
		}
	}

	public class With_Empty_Args
	{
		[Fact]
		public void Unsets_Args()
		{
			// Arrange
			var failure = FailGen.Create();

			// Act
			var result = failure.Arg([]);

			// Assert
			Assert.Empty(result.Value.Args);
		}
	}

	public class With_Args
	{
		[Fact]
		public void Adds_Args_To_FailureValue()
		{
			// Arrange
			var failure = FailGen.Create();
			var args = new object?[] { Rnd.Int, Rnd.Str, Rnd.Date };

			// Act
			var result = failure.Arg(args);

			// Assert
			Assert.Equal(args, result.Value.Args);
		}
	}
}
