// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap.Extensions.ResultExtensions_Tests;

public class GetSingle_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<List<int>>(new(value));

			// Act
			var result = input.GetSingle<List<int>, int>();

			// Assert
			result.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		public class Empty_List
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<int>());

				// Act
				var result = input.GetSingle<List<int>, int>();

				// Assert
				result.AssertFailure("Cannot get single value from an empty list.");
			}
		}

		public class Multiple_Values
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<int> { Rnd.Int, Rnd.Int });

				// Act
				var result = input.GetSingle<List<int>, int>();

				// Assert
				result.AssertFailure("Cannot get single value from a list with multiple values.");
			}
		}

		public class Single_Value
		{
			[Fact]
			public void Returns_Ok_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(new List<int> { value });

				// Act
				var result = input.GetSingle<List<int>, int>();

				// Assert
				result.AssertOk(value);
			}
		}

		public class Incorrect_Element_Type
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<string> { Rnd.Str });

				// Act
				var result = input.GetSingle<List<string>, int>();

				// Assert
				result.AssertFailure();
			}
		}
	}

	public class With_OnError_Handler
	{
		[Fact]
		public void Handler_Is_Called_When_List_Is_Empty()
		{
			// Arrange
			var input = R.Wrap(new List<int>());
			var handlerCalled = false;
			R.ErrorHandler handler = (_, __) => { handlerCalled = true; return FailGen.Create(); };

			// Act
			_ = input.GetSingle<List<int>, int>(handler);

			// Assert
			Assert.True(handlerCalled);
		}

		[Fact]
		public void Handler_Result_Is_Returned_When_List_Is_Empty()
		{
			// Arrange
			var customMessage = Rnd.Str;
			var input = R.Wrap(new List<int>());
			R.ErrorHandler handler = (_, __) => FailGen.Create(new(customMessage));

			// Act
			var result = input.GetSingle<List<int>, int>(handler);

			// Assert
			result.AssertFailure(customMessage);
		}

		[Fact]
		public void Handler_Is_Called_When_List_Has_Multiple_Values()
		{
			// Arrange
			var input = R.Wrap(new List<int> { Rnd.Int, Rnd.Int });
			var handlerCalled = false;
			R.ErrorHandler handler = (_, __) => { handlerCalled = true; return FailGen.Create(); };

			// Act
			_ = input.GetSingle<List<int>, int>(handler);

			// Assert
			Assert.True(handlerCalled);
		}

		[Fact]
		public void Handler_Is_Not_Called_When_Single_Value()
		{
			// Arrange
			var value = Rnd.Int;
			var input = R.Wrap(new List<int> { value });
			var handlerCalled = false;
			R.ErrorHandler handler = (_, __) => { handlerCalled = true; return FailGen.Create(); };

			// Act
			var result = input.GetSingle<List<int>, int>(handler);

			// Assert
			Assert.False(handlerCalled);
			result.AssertOk(value);
		}
	}

	public class Fluent_API
	{
		public class Single_Value
		{
			[Fact]
			public void Returns_Ok_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(new List<int> { value });

				// Act
				var result = input.GetSingle(f => f.Value<int>());

				// Assert
				result.AssertOk(value);
			}
		}

		public class Empty_List
		{
			[Fact]
			public void Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<int>());

				// Act
				var result = input.GetSingle(f => f.Value<int>());

				// Assert
				result.AssertFailure("Cannot get single value from an empty list.");
			}
		}
	}
}
