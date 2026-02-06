// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class FilterMap_Tests0
{
	private static MaybeVars SetupMaybe(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var predicate = Substitute.For<Func<string, bool>>();
		predicate.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var map = Substitute.For<Func<string, Maybe<string>>>();

		var list = new[] { Rnd.Str, Rnd.Str, Rnd.Str };

		return new(GetMaybe(withValues ? list : null, mixed), predicate, map, list);
	}

	private static IEnumerable<string> GetMaybe(string[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return values[i];
			}
			else
			{
				yield return null!;
			}

			if (mixed)
			{
				yield return null!;
			}
		}
	}

	public class With_Null_Values
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

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
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, false);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Values
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public void Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, true);

				// Act
				var result = v.List.FilterMap(v.Predicate, v.Map);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, true);

				// Act
				_ = v.List.FilterMap(v.Predicate, v.Map);

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
				var result = v.List.FilterMap(v.Predicate, v.Map);

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
		IEnumerable<string> List,
		Func<string, bool> Predicate,
		Func<string, Maybe<string>> Map,
		string[] Values
	);
}
