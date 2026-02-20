// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterAsync_Tests
{
	public class No_Predicate
	{
		public class With_None
		{
			[Fact]
			public async Task Returns_Empty_List()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];

				// Act
				var result = await list.AsTask().FilterAsync();

				// Assert
				Assert.Empty(result);
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Returns_All_Values()
			{
				// Arrange
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				IEnumerable<Maybe<int>> list = [v0, v1, v2];

				// Act
				var result = await list.AsTask().FilterAsync();

				// Assert
				Assert.Collection(result,
					x => x.AssertSome(v0),
					x => x.AssertSome(v1),
					x => x.AssertSome(v2)
				);
			}
		}

		public class With_Mixed
		{
			[Fact]
			public async Task Removes_None()
			{
				// Arrange
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				IEnumerable<Maybe<int>> list = [v0, M.None, v1, M.None];

				// Act
				var result = await list.AsTask().FilterAsync();

				// Assert
				Assert.Collection(result,
					x => x.AssertSome(v0),
					x => x.AssertSome(v1)
				);
			}
		}
	}

	public class With_Predicate
	{
		public class With_None
		{
			[Fact]
			public async Task Does_Not_Call_Predicate()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];
				var fTest = Substitute.For<Func<int, Task<bool>>>();

				// Act - canonical overload
				_ = await list.AsTask().FilterAsync(fTest);

				// Assert
				await fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}

			[Fact]
			public async Task Returns_Empty_List()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];
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

		public class With_Some
		{
			public class Predicate_Returns_False
			{
				[Fact]
				public async Task Returns_Empty_List()
				{
					// Arrange
					IEnumerable<Maybe<int>> list = [Rnd.Int, Rnd.Int, Rnd.Int];
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
					IEnumerable<Maybe<int>> list = [v0, v1, v2];
					var fTest = Substitute.For<Func<int, Task<bool>>>();
					fTest.Invoke(Arg.Any<int>()).Returns(Task.FromResult(true));

					// Act - canonical async overload
					var r0 = await list.AsTask().FilterAsync(fTest);

					// Act - sync fTest overload (IEnumerable source)
					var r1 = await list.FilterAsync(fTest);

					// Assert
					Assert.Collection(r0,
						x => x.AssertSome(v0),
						x => x.AssertSome(v1),
						x => x.AssertSome(v2)
					);
					Assert.Collection(r1,
						x => x.AssertSome(v0),
						x => x.AssertSome(v1),
						x => x.AssertSome(v2)
					);
				}
			}
		}
	}
}
