// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterMap_Tests
{
	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var map = Substitute.For<Func<string, Result<string>>>();

		var list = new[] { Rnd.Str, Rnd.Str, Rnd.Str };

		return new([.. GetResult(withValues ? list : null, mixed)], fTest, map, list);
	}

	private static IEnumerable<Result<string>> GetResult(string[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return values[i];
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
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				_ = v.List.FilterMap(v.Test, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				_ = v.List.FilterMap(v.Test, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, true);

				// Act
				_ = v.List.FilterMap(v.Test, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Ok_With_Value()
			{
				// Arrange
				var v = SetupResult(true, true);
				v.Map.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToString());

				// Act
				var result = v.List.FilterMap(v.Test, v.Map);

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
		IEnumerable<Result<string>> List,
		Func<string, bool> Test,
		Func<string, Result<string>> Map,
		string[] Values
	);
}
