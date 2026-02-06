// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterMapAsync_Tests
{
	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var predicate = Substitute.For<Func<int, bool>>();
		var predicateAsync = Substitute.For<Func<int, Task<bool>>>();
		predicate.Invoke(Arg.Any<int>()).Returns(predicateReturn);
		predicateAsync.Invoke(Arg.Any<int>()).Returns(predicateReturn);

		var list = new[] { Rnd.Int, Rnd.Int, Rnd.Int };

		return new(
			[.. GetResult(withValues ? list : null, mixed)],
			predicate,
			predicateAsync,
			Substitute.For<Func<int, string>>(),
			Substitute.For<Func<int, Task<string>>>(),
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
			public async Task Returns_Original_Failures()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				var r1 = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				var r2 = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				var r3 = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				var r4 = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				var r5 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				var r6 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				Bind_Tests.AssertFailures([.. v.List], r0);
				Bind_Tests.AssertFailures([.. v.List], r1);
				Bind_Tests.AssertFailures([.. v.List], r2);
				Bind_Tests.AssertFailures([.. v.List], r3);
				Bind_Tests.AssertFailures([.. v.List], r4);
				Bind_Tests.AssertFailures([.. v.List], r5);
				Bind_Tests.AssertFailures([.. v.List], r6);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(false, false);

				// Act
				_ = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Original_Failures()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				var r1 = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				var r2 = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				var r3 = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				var r4 = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				var r5 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				var r6 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				Bind_Tests.AssertFailures([.. v.List], r0);
				Bind_Tests.AssertFailures([.. v.List], r1);
				Bind_Tests.AssertFailures([.. v.List], r2);
				Bind_Tests.AssertFailures([.. v.List], r3);
				Bind_Tests.AssertFailures([.. v.List], r4);
				Bind_Tests.AssertFailures([.. v.List], r5);
				Bind_Tests.AssertFailures([.. v.List], r6);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(true, false);

				// Act
				_ = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}
	}

	public class With_Ok
	{
		public class Predicate_Returns_False
		{
			[Fact]
			public async Task Returns_Predicate_Was_False_Failures()
			{
				// Arrange
				var v = SetupResult(false, true);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				var r1 = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				var r2 = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				var r3 = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				var r4 = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				var r5 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				var r6 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				FilterBind_Tests.AssertFailures(r0);
				FilterBind_Tests.AssertFailures(r1);
				FilterBind_Tests.AssertFailures(r2);
				FilterBind_Tests.AssertFailures(r3);
				FilterBind_Tests.AssertFailures(r4);
				FilterBind_Tests.AssertFailures(r5);
				FilterBind_Tests.AssertFailures(r6);
			}

			[Fact]
			public async Task Function_Is_Not_Invoked()
			{
				// Arrange
				var v = SetupResult(false, true);

				// Act
				_ = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Some_With_Value()
			{
				// Arrange
				var v = SetupResult(true, true);
				v.Map.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				v.MapAsync.Invoke(Arg.Any<int>()).Returns(c => c.Arg<int>().ToString());
				var assertCollection = (IEnumerable<Result<string>> list) =>
					Assert.Collection(list,
						x => x.AssertOk(v.Values[0].ToString()),
						x => x.AssertOk(v.Values[1].ToString()),
						x => x.AssertOk(v.Values[2].ToString())
					);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				var r1 = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				var r2 = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				var r3 = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				var r4 = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				var r5 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				var r6 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				assertCollection(r0);
				assertCollection(r1);
				assertCollection(r2);
				assertCollection(r3);
				assertCollection(r4);
				assertCollection(r5);
				assertCollection(r6);
			}
		}
	}

	internal record class ResultVars(
		IEnumerable<Result<int>> List,
		Func<int, bool> Predicate,
		Func<int, Task<bool>> PredicateAsync,
		Func<int, string> Map,
		Func<int, Task<string>> MapAsync,
		int[] Values
	)
	{
		public Task<IEnumerable<Result<int>>> ListAsync =>
			Task.FromResult(List);
	}
}
