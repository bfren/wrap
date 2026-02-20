// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class IterateAsync_Tests
{
	public class With_None
	{
		[Fact]
		public async Task Does_Not_Call_Function()
		{
			// Arrange
			IEnumerable<Maybe<Guid>> list = [M.None, M.None, M.None];
			var f = Substitute.For<Func<Guid, Task>>();
			f.Invoke(Arg.Any<Guid>()).Returns(Task.CompletedTask);

			// Act - IEnumerable source overload
			await list.IterateAsync(f);

			// Act - Task source overload
			await list.AsTask().IterateAsync(f);

			// Assert
			await f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Guid>());
		}

		[Fact]
		public async Task With_Action_Does_Not_Call_Function()
		{
			// Arrange
			IEnumerable<Maybe<Guid>> list = [M.None, M.None, M.None];
			var f = Substitute.For<Action<Guid>>();

			// Act
			await list.AsTask().IterateAsync(f);

			// Assert
			f.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<Guid>());
		}
	}

	public class With_Some
	{
		[Fact]
		public async Task Calls_Function_With_Value()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Guid;
			var v2 = Rnd.Guid;
			IEnumerable<Maybe<Guid>> list = [v0, v1, v2];
			var f = Substitute.For<Func<Guid, Task>>();
			f.Invoke(Arg.Any<Guid>()).Returns(Task.CompletedTask);

			// Act - canonical async overload (Task source)
			await list.AsTask().IterateAsync(f);

			// Assert
			await f.Received().Invoke(v0);
			await f.Received().Invoke(v1);
			await f.Received().Invoke(v2);
		}

		[Fact]
		public async Task With_IEnumerable_Source_Calls_Function_With_Value()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Guid;
			var v2 = Rnd.Guid;
			IEnumerable<Maybe<Guid>> list = [v0, v1, v2];
			var f = Substitute.For<Func<Guid, Task>>();
			f.Invoke(Arg.Any<Guid>()).Returns(Task.CompletedTask);

			// Act
			await list.IterateAsync(f);

			// Assert
			await f.Received().Invoke(v0);
			await f.Received().Invoke(v1);
			await f.Received().Invoke(v2);
		}

		[Fact]
		public async Task With_Action_Calls_Function_With_Value()
		{
			// Arrange
			var v0 = Rnd.Guid;
			var v1 = Rnd.Guid;
			var v2 = Rnd.Guid;
			IEnumerable<Maybe<Guid>> list = [v0, v1, v2];
			var f = Substitute.For<Action<Guid>>();

			// Act
			await list.AsTask().IterateAsync(f);

			// Assert
			f.Received().Invoke(v0);
			f.Received().Invoke(v1);
			f.Received().Invoke(v2);
		}
	}
}
