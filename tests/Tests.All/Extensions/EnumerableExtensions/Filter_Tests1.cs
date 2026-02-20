// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Filter_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Does_Not_Call_Predicate()
		{
			// Arrange
			IEnumerable<Result<int>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
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
			IEnumerable<Result<int>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var fTest = Substitute.For<Func<int, bool>>();

			// Act
			var result = list.Filter(fTest);

			// Assert
			Assert.Empty(result);
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty()
			{
				// Arrange
				IEnumerable<Result<int>> list = [R.Wrap(Rnd.Int), R.Wrap(Rnd.Int), R.Wrap(Rnd.Int)];
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
				IEnumerable<Result<int>> list = [R.Wrap(v0), R.Wrap(v1), R.Wrap(v2)];
				var fTest = Substitute.For<Func<int, bool>>();
				fTest.Invoke(Arg.Any<int>()).Returns(true);

				// Act
				var result = list.Filter(fTest);

				// Assert
				Assert.Collection(result,
					x => x.AssertOk(v0),
					x => x.AssertOk(v1),
					x => x.AssertOk(v2)
				);
			}
		}
	}
}
