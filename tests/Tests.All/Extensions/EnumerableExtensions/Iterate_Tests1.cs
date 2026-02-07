// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Iterate_Tests
{
	public class With_Failure
	{
		[Fact]
		public void Does_Not_Call_Function()
		{
			// Arrange
			IEnumerable<Result<Guid>> list = [FailGen.Create(), FailGen.Create(), FailGen.Create()];
			var f = Substitute.For<Action<Guid>>();

			// Act
			list.Iterate(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Guid>());
		}
	}

	public class With_Ok
	{
		[Fact]
		public void Calls_Function_With_Value()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Guid;
			var v2 = Rnd.Guid;
			IEnumerable<Result<Guid>> list = [v0, v1, v2];
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
