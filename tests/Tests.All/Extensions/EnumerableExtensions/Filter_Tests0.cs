// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Filter_Tests
{
	public class No_Predicate
	{
		public class With_None
		{
			[Fact]
			public void Returns_Empty()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];

				// Act
				var result = list.Filter();

				// Assert
				Assert.Empty(result);
			}
		}

		public class With_Some
		{
			[Fact]
			public void Returns_All_Values()
			{
				// Arrange
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				IEnumerable<Maybe<int>> list = [v0, v1, v2];

				// Act
				var result = list.Filter();

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
			public void Removes_None()
			{
				// Arrange
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				IEnumerable<Maybe<int>> list = [v0, M.None, v1, M.None];

				// Act
				var result = list.Filter();

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
			public void Does_Not_Call_Predicate()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];
				var fTest = Substitute.For<Func<int, bool>>();

				// Act
				_ = list.Filter(fTest);

				// Assert
				fTest.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}

			[Fact]
			public void Returns_Empty()
			{
				// Arrange
				IEnumerable<Maybe<int>> list = [M.None, M.None, M.None];
				var fTest = Substitute.For<Func<int, bool>>();

				// Act
				var result = list.Filter(fTest);

				// Assert
				Assert.Empty(result);
			}
		}

		public class With_Some
		{
			public class Predicate_Returns_False
			{
				[Fact]
				public void Returns_Empty()
				{
					// Arrange
					IEnumerable<Maybe<int>> list = [Rnd.Int, Rnd.Int, Rnd.Int];
					var fTest = Substitute.For<Func<int, bool>>();
					fTest.Invoke(Arg.Any<int>()).Returns(false);

					// Act
					var result = list.Filter(fTest);

					// Assert
					Assert.Empty(result);
				}
			}

			public class Predicate_Returns_True
			{
				[Fact]
				public void Returns_Matching_Values()
				{
					// Arrange
					var v0 = Rnd.Int;
					var v1 = Rnd.Int;
					var v2 = Rnd.Int;
					IEnumerable<Maybe<int>> list = [v0, v1, v2];
					var fTest = Substitute.For<Func<int, bool>>();
					fTest.Invoke(Arg.Any<int>()).Returns(true);

					// Act
					var result = list.Filter(fTest);

					// Assert
					Assert.Collection(result,
						x => x.AssertSome(v0),
						x => x.AssertSome(v1),
						x => x.AssertSome(v2)
					);
				}
			}
		}
	}
}
