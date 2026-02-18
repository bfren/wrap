// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

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
				r0.AssertFailure("Cannot get single value from an empty list.");
				r1.AssertFailure("Cannot get single value from an empty list.");
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
				r0.AssertFailure("Cannot get single value from a list with multiple values.");
				r1.AssertFailure("Cannot get single value from a list with multiple values.");
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
