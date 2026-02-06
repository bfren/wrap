// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterMap_Tests
{
	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var predicate = Substitute.For<Func<int, bool>>();
		predicate.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var map = Substitute.For<Func<int, string>>();

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new([.. GetResult(withValues ? list : null, mixed)], predicate, map, list);
	}

	private static IEnumerable<Result<int>> GetResult(int[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return R.Wrap(values[i]);
			}
			else
			{
				yield return FailGen.Create();
			}

			if (mixed)
			{
				yield return FailGen.Create();
			}
		}
	}

	public class With_Failure
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Original_Failures()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Bind_Tests.AssertFailures([.. v.List], result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Original_Failures()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Bind_Tests.AssertFailures([.. v.List], result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Failures()
			{
				// Arrange
				var v = SetupResult(false, true);

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				FilterBind_Tests.AssertFailures(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, true);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Ok_With_Value()
			{
				// Arrange
				var v = SetupResult(true, true);
				v.Map.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Assert.Collection(result,
					x => x.AssertOk(v.Values[0].ToString()),
					x => x.AssertOk(v.Values[1].ToString()),
					x => x.AssertOk(v.Values[2].ToString())
				);
			}
		}
	}

	private sealed record class ResultVars(
		IEnumerable<Result<int>> List,
		Func<int, bool> Predicate,
		Func<int, string> Map,
		int[] Values
	);
}
