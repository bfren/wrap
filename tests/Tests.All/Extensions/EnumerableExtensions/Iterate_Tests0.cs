// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Iterate_Tests
{
	public class With_None
	{
		[Fact]
		public void Does_Not_Call_Function()
		{
			// Arrange
			IEnumerable<Maybe<Guid>> list = [M.None, M.None, M.None];
			var f = Substitute.For<Action<Guid>>();

			// Act
			list.Iterate(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Guid>());
		}
	}

	public class With_Some
	{
		[Fact]
		public void Calls_Function_With_Value()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Guid;
			var v2 = Rnd.Guid;
			IEnumerable<Maybe<Guid>> list = [v0, v1, v2];
			var f = Substitute.For<Action<Guid>>();

			// Act
			list.Iterate(f);

			// Assert
			f.Received().Invoke(v0);
			f.Received().Invoke(v1);
			f.Received().Invoke(v2);
		}
	}
}
