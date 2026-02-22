// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class FilterMapAsync_Tests
{
	private static MaybeVars SetupMaybe(bool predicateReturn, bool withValues, bool mixed = false)
	{
		var fTest = Substitute.For<Func<string, bool>>();
		var predicateAsync = Substitute.For<Func<string, Task<bool>>>();
		fTest.Invoke(Arg.Any<string>()).Returns(predicateReturn);

		var list = new[] { Rnd.Str, Rnd.Str, Rnd.Str };

		return new(
			GetMaybe(withValues ? list : null, mixed),
			fTest,
			Substitute.For<Func<string, string>>(),
			Substitute.For<Func<string, Task<string>>>(),
			list
		);
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
			public async Task Returns_Empty_List()
			{
				// Arrange
				var v = SetupMaybe(false, false);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				var r1 = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				var r2 = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

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
				_ = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

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
				var v = SetupMaybe(true, false);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				var r1 = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				var r2 = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

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
				_ = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

				// Assert
				v.Map.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				await v.MapAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
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
				var r0 = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				var r1 = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				var r2 = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

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
				_ = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				_ = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

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
				var v = SetupMaybe(true, true);
				v.Map.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToString());
				v.MapAsync.Invoke(Arg.Any<string>()).Returns(c => c.Arg<string>().ToString());
				var assertCollection = (IEnumerable<Maybe<string>> list) =>
					Assert.Collection(list,
						x => x.AssertSome(v.Values[0].ToString()),
						x => x.AssertSome(v.Values[1].ToString()),
						x => x.AssertSome(v.Values[2].ToString())
					);

				// Act
				var r0 = await v.List.FilterMapAsync(v.Test, v.MapAsync);
				var r1 = await v.ListAsync.FilterMapAsync(v.Test, v.Map);
				var r2 = await v.ListAsync.FilterMapAsync(v.Test, v.MapAsync);

				// Assert
				assertCollection(r0);
				assertCollection(r1);
				assertCollection(r2);
			}
		}
	}

	internal sealed record class MaybeVars(
		IEnumerable<Maybe<string>> List,
		Func<string, bool> Test,
		Func<string, string> Map,
		Func<string, Task<string>> MapAsync,
		string[] Values
	)
	{
		public Task<IEnumerable<Maybe<string>>> ListAsync =>
			Task.FromResult(List);
	}
}
