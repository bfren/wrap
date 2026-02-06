// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class FilterMapAsync_Tests1
{
	private static ResultVars SetupResult(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var predicate = Substitute.For<Func<string, bool>>();
		var predicateAsync = Substitute.For<Func<string, Task<bool>>>();
		predicate.Invoke(Arg.Any<string>()).Returns(predicateReturn);
		predicateAsync.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var list = new[] { Rnd.Str, Rnd.Str, Rnd.Str };

		return new(
			[.. GetResult(withValues ? list : null, mixed)],
			predicate,
			predicateAsync,
			Substitute.For<Func<string, Result<string>>>(),
			Substitute.For<Func<string, Task<Result<string>>>>(),
			list
		);
	}

	private static IEnumerable<string> GetResult(string[]? values, bool mixed)
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
			public async Task Returns_Empty_List()
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
				Assert.Empty(r0);
				Assert.Empty(r1);
				Assert.Empty(r2);
				Assert.Empty(r3);
				Assert.Empty(r4);
				Assert.Empty(r5);
				Assert.Empty(r6);
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
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var r0 = await v.List.FilterMapAsync(v.Predicate, v.MapAsync);
				var r1 = await v.List.FilterMapAsync(v.PredicateAsync, v.Map);
				var r2 = await v.List.FilterMapAsync(v.PredicateAsync, v.MapAsync);
				var r3 = await v.ListAsync.FilterMapAsync(v.Predicate, v.Map);
				var r4 = await v.ListAsync.FilterMapAsync(v.Predicate, v.MapAsync);
				var r5 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.Map);
				var r6 = await v.ListAsync.FilterMapAsync(v.PredicateAsync, v.MapAsync);

				// Assert
				Assert.Empty(r0);
				Assert.Empty(r1);
				Assert.Empty(r2);
				Assert.Empty(r3);
				Assert.Empty(r4);
				Assert.Empty(r5);
				Assert.Empty(r6);
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
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}
	}

	public class With_Values
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
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}
		}

		public class Predicate_Returns_True
		{
			[Fact]
			public async Task Returns_Some_With_Value()
			{
				// Arrange
				var v = SetupResult(true, true);
				v.Map.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToString());
				v.MapAsync.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToString());
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
		IEnumerable<string> List,
		Func<string, bool> Predicate,
		Func<string, Task<bool>> PredicateAsync,
		Func<string, Result<string>> Map,
		Func<string, Task<Result<string>>> MapAsync,
		string[] Values
	)
	{
		public Task<IEnumerable<string>> ListAsync =>
			Task.FromResult(List);
	}
}
