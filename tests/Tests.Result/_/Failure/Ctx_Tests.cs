// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Failure_Tests;

public class Ctx_Tests
{
	public class With_Class_And_Function
	{
		[Fact]
		public void Adds_Class_And_Function_To_Context()
		{
			// Arrange
			var failure = new Failure(Rnd.Str);

			// Act
			var result = failure.Ctx(nameof(With_Class_And_Function), nameof(Adds_Class_And_Function_To_Context));

			// Assert
			Assert.Equal(
				$"{nameof(With_Class_And_Function)}.{nameof(Adds_Class_And_Function_To_Context)}()",
				result.Value.Context
			);
		}
	}

	public class With_Generic
	{
		[Fact]
		public void Adds_Generic_Type_Name_To_Context()
		{
			// Arrange
			var failure = new Failure(Rnd.Str);

			// Act
			var result = failure.Ctx<Ctx_Tests>();

			// Assert
			Assert.Equal($"{typeof(Ctx_Tests).FullName}", result.Value.Context);
		}
	}
}
