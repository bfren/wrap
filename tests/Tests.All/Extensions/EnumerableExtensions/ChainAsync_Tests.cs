// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public class ChainAsync_Tests
{
	[Fact]
	public async Task Async_Steps_Chain_Without_Leaving_The_Railway()
	{
		// Arrange
		var list = Task.FromResult<IEnumerable<Maybe<int>>>([M.Wrap(1), M.None, M.Wrap(2)]);

		// Act
		var result = await list
			.BindAsync(x => M.Wrap(x * 10).AsTask())
			.MapAsync(x => x + 1);

		// Assert
		Assert.Collection(result,
			x => x.AssertSome(11),
			x => x.AssertSome(21)
		);
	}

	[Fact]
	public async Task Result_Read_Twice__Runs_f_Once_Per_Item()
	{
		// Arrange
		var list = Task.FromResult<IEnumerable<Maybe<int>>>([M.Wrap(1), M.Wrap(2)]);
		var f = Substitute.For<Func<int, int>>();

		// Act
		var result = await list.MapAsync(f);
		_ = result.Count();
		_ = result.Count();

		// Assert
		f.ReceivedWithAnyArgs(2).Invoke(default);
	}
}
