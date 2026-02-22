// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterBindAsync_Tests
{
	private static MaybeVars SetupMaybe(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<int, bool>>();
		var predicateAsync = Substitute.For<Func<int, Task<bool>>>();
		fTest.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new(
			GetMaybe(withValues ? list : null, mixed),
			fTest,
			Substitute.For<Func<int, Maybe<string>>>(),
			Substitute.For<Func<int, Task<Maybe<string>>>>(),
			list
		);
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
			public async Task Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				var r0 = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				var r1 = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				var r2 = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				Assert.Empty(r0);
				Assert.Empty(r1);
				Assert.Empty(r2);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				_ = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.BindAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(true, false);

				// Act
				var r0 = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				var r1 = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				var r2 = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				Assert.Empty(r0);
				Assert.Empty(r1);
				Assert.Empty(r2);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(true, false);

				// Act
				_ = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.BindAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
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
				var v = SetupMaybe(false, true);

				// Act
				var r0 = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				var r1 = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				var r2 = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				Assert.Empty(r0);
				Assert.Empty(r1);
				Assert.Empty(r2);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupMaybe(false, true);

				// Act
				_ = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				_ = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.BindAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Some_With_Value()
			{
				// Arrange
				var v = SetupMaybe(true, true);
				v.Bind.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				v.BindAsync.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				var assertCollection = (IEnumerable<Maybe<string>> list) =>
					Assert.Collection(list,
						x => x.AssertSome(v.Values[0].ToString()),
						x => x.AssertSome(v.Values[1].ToString()),
						x => x.AssertSome(v.Values[2].ToString())
					);

				// Act
				var r0 = await v.List.FilterBindAsync(v.Test, v.BindAsync);
				var r1 = await v.ListAsync.FilterBindAsync(v.Test, v.Bind);
				var r2 = await v.ListAsync.FilterBindAsync(v.Test, v.BindAsync);

				// Assert
				assertCollection(r0);
				assertCollection(r1);
				assertCollection(r2);
			}
		}
	}

	internal sealed record class MaybeVars(
		IEnumerable<Maybe<int>> List,
		Func<int, bool> Test,
		Func<int, Maybe<string>> Bind,
		Func<int, Task<Maybe<string>>> BindAsync,
		int[] Values
	)
	{
		public Task<IEnumerable<Maybe<int>>> ListAsync =>
			Task.FromResult(List);
	}
}
