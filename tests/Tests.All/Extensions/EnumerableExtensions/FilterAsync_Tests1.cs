// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterAsync_Tests
{
	public class With_Failure
	{
		[Fact]
		public async Task Does_Not_Call_Predicate()
		{
			// Arrange
			IEnumerable<Result<int>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var fTest = Substitute.For<Func<int, Task<bool>>>();

			// Act
			_ = await list.AsTask().FilterAsync(fTest);

			// Assert
			await fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public async Task Returns_Empty_List()
		{
			// Arrange
			IEnumerable<Result<int>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var fTest = Substitute.For<Func<int, Task<bool>>>();

			// Act - canonical async overload
			var r0 = await list.AsTask().FilterAsync(fTest);

			// Act - sync fTest overload (IEnumerable source)
			var r1 = await list.FilterAsync(fTest);

			// Assert
			Assert.Empty(r0);
			Assert.Empty(r1);
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Empty_List()
			{
				// Arrange
				IEnumerable<Result<int>> list = [R.Wrap(Rnd.Int), R.Wrap(Rnd.Int), R.Wrap(Rnd.Int)];
				var fTest = Substitute.For<Func<int, Task<bool>>>();
				fTest.Invoke(Arg.Any<int>()).Returns(Task.FromResult(false));

				// Act - canonical async overload
				var r0 = await list.AsTask().FilterAsync(fTest);

				// Act - sync fTest overload (Task source)
				var r1 = await list.AsTask().FilterAsync(x => false);

				// Assert
				Assert.Empty(r0);
				Assert.Empty(r1);
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Matching_Values()
			{
				// Arrange
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				IEnumerable<Result<int>> list = [R.Wrap(v0), R.Wrap(v1), R.Wrap(v2)];
				var fTest = Substitute.For<Func<int, Task<bool>>>();
				fTest.Invoke(Arg.Any<int>()).Returns(Task.FromResult(true));

				// Act - canonical async overload
				var r0 = await list.AsTask().FilterAsync(fTest);

				// Act - sync fTest overload (IEnumerable source)
				var r1 = await list.FilterAsync(fTest);

				// Assert
				Assert.Collection(r0,
					x => x.AssertOk(v0),
					x => x.AssertOk(v1),
					x => x.AssertOk(v2)
				);
				Assert.Collection(r1,
					x => x.AssertOk(v0),
					x => x.AssertOk(v1),
					x => x.AssertOk(v2)
				);
			}
		}
	}
}
