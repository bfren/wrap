// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.EnumerableExtensions_Tests;

public partial class Bind_Tests
{
	public class With_None
	{
		[Fact]
		public void Does_Not_Call_Bind_Function()
		{
			// Arrange
			var list = new[] { NoneGen.Create<int>(), NoneGen.Create<int>(), NoneGen.Create<int>() };
			var bind = Substitute.For<Func<int, Maybe<string>>>();

			// Act
			_ = list.Bind(bind);

			// Assert
			bind.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<int>());
		}

		[Fact]
		public void Returns_Empty_List()
		{
			// Arrange
			var list = new[] { NoneGen.Create<int>(), NoneGen.Create<int>(), NoneGen.Create<int>() };

			// Act
			var result = list.Bind(Substitute.For<Func<int, Maybe<string>>>());

			// Assert
			Assert.Empty(result);
		}
	}

	public class With_Some
	{
		[Fact]
		public void Returns_Values()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			var list = new[] { M.Wrap(v0), M.Wrap(v1), M.Wrap(v2) };

			// Act
			var result = list.Bind(x => M.Wrap(x.ToString()));

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0.ToString(), x),
				x => Assert.Equal(v1.ToString(), x),
				x => Assert.Equal(v2.ToString(), x)
			);
		}
	}

	public partial class With_Mixed
	{
		[Fact]
		public void Removes_None()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			var list = new[] { M.Wrap(v0), M.None, M.Wrap(v1), M.None, M.Wrap(v2) };

			// Act
			var result = list.Bind(x => M.Wrap(x.ToString()));

			// Assert
			Assert.Equal(3, result.Count());
		}

		[Fact]
		public void Returns_Some()
		{
			// Arrange
			var v0 = Rnd.Int;
			var v1 = Rnd.Int;
			var v2 = Rnd.Int;
			var list = new[] { M.Wrap(v0), M.None, M.Wrap(v1), M.None, M.Wrap(v2) };

			// Act
			var result = list.Bind(x => M.Wrap(x.ToString()));

			// Assert
			Assert.Collection(result,
				x => Assert.Equal(v0.ToString(), x),
				x => Assert.Equal(v1.ToString(), x),
				x => Assert.Equal(v2.ToString(), x)
			);
		}
	}
}
