// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterBind_Tests
{
	internal static void AssertFailures<T>(IEnumerable<Result<T>> actual) =>
		Assert.Collection(actual,
			x => x.AssertFailure(C.TestFalseMessage),
			x => x.AssertFailure(C.TestFalseMessage),
			x => x.AssertFailure(C.TestFalseMessage)
		);

	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<int, bool>>();
		fTest.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<int, Result<string>>>();

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new([.. GetResult(withValues ? list : null, mixed)], fTest, bind, list);
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
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				var result = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				_ = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				var result = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				_ = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupResult(false, true);

				// Act
				var result = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, true);

				// Act
				_ = v.List.FilterBind(v.Test, v.Bind);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Ok_With_Value()
			{
				// Arrange
				var v = SetupResult(true, true);
				v.Bind.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());

				// Act
				var result = v.List.FilterBind(v.Test, v.Bind);

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
		Func<int, bool> Test,
		Func<int, Result<string>> Bind,
		int[] Values
	);
}
