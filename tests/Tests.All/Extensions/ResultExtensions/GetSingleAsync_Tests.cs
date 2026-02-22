// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.ResultExtensions_Tests;

public class GetSingleAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Returns_Failure()
		{
			// Arrange
			var value = Rnd.Str;
			var input = FailGen.Create<List<int>>(new(value));

			// Act
			var r0 = await input.AsTask().GetSingleAsync(f => f.Value<int>());
			var r1 = await input.AsTask().GetSingleAsync(f => f.Value<int>(), null);

			// Assert
			r0.AssertFailure(value);
			r1.AssertFailure(value);
		}
	}

	public class With_Ok
	{
		public class Empty_List
		{
			[Fact]
			public async Task Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<int>());

				// Act
				var r0 = await input.AsTask().GetSingleAsync(f => f.Value<int>());
				var r1 = await input.AsTask().GetSingleAsync(f => f.Value<int>(), null);

				// Assert
				r0.AssertFailure(C.GetSingle.EmptyList);
				r1.AssertFailure(C.GetSingle.EmptyList);
			}
		}

		public class Multiple_Values
		{
			[Fact]
			public async Task Returns_Failure()
			{
				// Arrange
				var input = R.Wrap(new List<int> { Rnd.Int, Rnd.Int });

				// Act
				var r0 = await input.AsTask().GetSingleAsync(f => f.Value<int>());
				var r1 = await input.AsTask().GetSingleAsync(f => f.Value<int>(), null);

				// Assert
				r0.AssertFailure(C.GetSingle.MultipleValues);
				r1.AssertFailure(C.GetSingle.MultipleValues);
			}
		}

		public class Single_Value
		{
			[Fact]
			public async Task Returns_Ok_With_Value()
			{
				// Arrange
				var value = Rnd.Int;
				var input = R.Wrap(new List<int> { value });

				// Act
				var r0 = await input.AsTask().GetSingleAsync(f => f.Value<int>());
				var r1 = await input.AsTask().GetSingleAsync(f => f.Value<int>(), null);

				// Assert
				r0.AssertOk(value);
				r1.AssertOk(value);
			}
		}
	}
}
