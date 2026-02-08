// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterBindAsync_Tests
{
	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<int, bool>>();
		var predicateAsync = Substitute.For<Func<int, Task<bool>>>();
		fTest.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new(
			[.. GetResult(withValues ? list : null, mixed)],
			fTest,
			Substitute.For<Func<int, Result<string>>>(),
			Substitute.For<Func<int, Task<Result<string>>>>(),
			list
		);
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
			public async Task Returns_Empty_List()
			{
				// Arrange
				var v = SetupResult(false, false);

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
				var v = SetupResult(false, false);

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
				var v = SetupResult(true, false);

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
				var v = SetupResult(true, false);

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

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Empty_List()
			{
				// Arrange
				var v = SetupResult(false, true);

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
				var v = SetupResult(false, true);

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
				var v = SetupResult(true, true);
				v.Bind.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				v.BindAsync.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				var assertCollection = (IEnumerable<Result<string>> list) =>
					Assert.Collection(list,
						x => x.AssertOk(v.Values[0].ToString()),
						x => x.AssertOk(v.Values[1].ToString()),
						x => x.AssertOk(v.Values[2].ToString())
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

	internal sealed record class ResultVars(
		IEnumerable<Result<int>> List,
		Func<int, bool> Test,
		Func<int, Result<string>> Bind,
		Func<int, Task<Result<string>>> BindAsync,
		int[] Values
	)
	{
		public Task<IEnumerable<Result<int>>> ListAsync =>
			Task.FromResult(List);
	}
}
