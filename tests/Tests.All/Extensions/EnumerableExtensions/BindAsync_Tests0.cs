// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class BindAsync_Tests
{
	private static MaybeVars SetupMaybe(bool withValues, bool mixed = false)
	{
		var values = new[] { Rnd.Int, Rnd.Int, Rnd.Int };
		return new(
			GetMaybe(withValues ? values : null, mixed),
			Substitute.For<Func<int, Maybe<string>>>(),
			Substitute.For<Func<int, Task<Maybe<string>>>>(),
			values
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
		[Fact]
		public async Task Does_Not_Call_Bind_Function()
		{
			// Arrange
			var v = SetupMaybe(false);

			// Act
			_ = await v.List.BindAsync(v.BindAsync);
			_ = await v.ListAsync.BindAsync(v.Bind);
			_ = await v.ListAsync.BindAsync(v.BindAsync);

			// Assert
			v.Bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
			await v.BindAsync.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public async Task Returns_Empty_List()
		{
			// Arrange
			var v = SetupMaybe(false);

			// Act
			var r0 = await v.List.BindAsync(v.BindAsync);
			var r1 = await v.ListAsync.BindAsync(v.Bind);
			var r2 = await v.ListAsync.BindAsync(v.BindAsync);

			// Assert
			Assert.Empty(r0);
			Assert.Empty(r1);
			Assert.Empty(r2);
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Returns_Values()
		{
			// Arrange
			var v = SetupMaybe(true);

			// Act
			var r0 = await v.List.BindAsync(x => M.Wrap(x.ToString()).AsTask());
			var r1 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()));
			var r2 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()).AsTask());

			// Assert
			Assert.Collection(r0,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r1,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r2,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
		}
	}

	public partial class With_Mixed
	{
		[Fact]
		public async Task Removes_None()
		{
			// Arrange
			var v = SetupMaybe(true, true);

			// Act
			var r0 = await v.List.BindAsync(x => M.Wrap(x.ToString()).AsTask());
			var r1 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()));
			var r2 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()).AsTask());

			// Assert
			Assert.Equal(3, r0.Count);
			Assert.Equal(3, r1.Count);
			Assert.Equal(3, r2.Count);
		}

		[Fact]
		public async Task Returns_Some()
		{
			// Arrange
			var v = SetupMaybe(true, true);

			// Act
			var r0 = await v.List.BindAsync(x => M.Wrap(x.ToString()).AsTask());
			var r1 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()));
			var r2 = await v.ListAsync.BindAsync(x => M.Wrap(x.ToString()).AsTask());

			// Assert
			Assert.Collection(r0,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r1,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
			Assert.Collection(r2,
				x => Assert.Equal(v.Values[0].ToString(), x),
				x => Assert.Equal(v.Values[1].ToString(), x),
				x => Assert.Equal(v.Values[2].ToString(), x)
			);
		}
	}

	internal sealed record class MaybeVars(
		IEnumerable<Maybe<int>> List,
		Func<int, Maybe<string>> Bind,
		Func<int, Task<Maybe<string>>> BindAsync,
		int[] Values
	)
	{
		public Task<IEnumerable<Maybe<int>>> ListAsync =>
			Task.FromResult(List);
	}
}
