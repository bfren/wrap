// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterBind_Tests
{
	private static MaybeVars SetupMaybe(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var predicate = Substitute.For<Func<int, bool>>();
		predicate.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var bind = Substitute.For<Func<int, Maybe<string>>>();

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new(GetMaybe(withValues ? list : null, mixed), predicate, bind, list);
	}

	private static IEnumerable<Maybe<int>> GetMaybe(int[]? values, bool mixed)
	{
		for (var i = 0; i < 3; i++)
		{
			if (values is not null)
			{
				yield return M.Wrap(values[i]);
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
				var result = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				_ = v.List.FilterBind(v.Predicate, v.Bind);

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
				var v = SetupMaybe(true, false);

				// Act
				var result = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, false);

				// Act
				_ = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
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
				var result = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				Assert.Empty(result);
			}

			[Fact]
			public void Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, true);

				// Act
				_ = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public void Returns_Some_With_Value()
			{
				// Arrange
				var v = SetupMaybe(true, true);
				v.Bind.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());

				// Act
				var result = v.List.FilterBind(v.Predicate, v.Bind);

				// Assert
				Assert.Collection(result,
					x => x.AssertSome(v.Values[0].ToString()),
					x => x.AssertSome(v.Values[1].ToString()),
					x => x.AssertSome(v.Values[2].ToString())
				);
			}
		}
	}

	private record class MaybeVars(
		IEnumerable<Maybe<int>> List,
		Func<int, bool> Predicate,
		Func<int, Maybe<string>> Bind,
		int[] Values
	);
}
