// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterMap_Tests
{
	private static MaybeVars SetupMaybe(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var map = Substitute.For<Func<string, Maybe<string>>>();

		var list = new[] { Rnd.Str, Rnd.Str, Rnd.Str };

		return new(GetMaybe(withValues ? list : null, mixed), fTest, map, list);
	}

	private static IEnumerable<Maybe<string>> GetMaybe(string[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return values[i];
			}
			else
			{
				yield return M.None;
			}

			if (mixed)
			{
				yield return M.None;
			}
		}
	}

	public class With_None
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(false, false);

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
				var v = SetupMaybe(true, false);

				// Act
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, false);

				// Act
				_ = v.List.FilterMap(v.Test, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Some
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, true);

				// Act
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, true);

				// Act
				_ = v.List.FilterMap(v.Test, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Some_With_Value()
			{
				// Arrange
				var v = SetupMaybe(true, true);
				v.Map.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToLower(F.DefaultCulture));

				// Act
				var result = v.List.FilterMap(v.Test, v.Map);

				// Assert
				Assert.Collection(result,
					x => x.AssertSome(v.Values[0].ToLower(F.DefaultCulture)),
					x => x.AssertSome(v.Values[1].ToLower(F.DefaultCulture)),
					x => x.AssertSome(v.Values[2].ToLower(F.DefaultCulture))
				);
			}
		}
	}

	private sealed record class MaybeVars(
		IEnumerable<Maybe<string>> List,
		Func<string, bool> Test,
		Func<string, Maybe<string>> Map,
		string[] Values
	);
}
