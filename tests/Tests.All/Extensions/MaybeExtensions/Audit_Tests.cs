// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class Audit_Tests
{
	public class With_FNone_Only
	{
		public class With_None
		{
			[Fact]
			public void Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();

				// Act
				_ = input.Audit(fNone);

				// Assert
				fNone.Received().Invoke();
			}

			[Fact]
			public void Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();

				// Act
				var result = input.Audit(fNone);

				// Assert
				result.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public void Does_Not_Call_FNone()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fNone = Substitute.For<Action>();

				// Act
				_ = input.Audit(fNone);

				// Assert
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public void Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();

				// Act
				var result = input.Audit(fNone);

				// Assert
				result.AssertSome(value);
			}
		}
	}

	public class With_FSome_Only
	{
		public class With_None
		{
			[Fact]
			public void Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fSome);

				// Assert
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}

			[Fact]
			public void Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = input.Audit(fSome);

				// Assert
				result.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public void Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fSome);

				// Assert
				fSome.Received().Invoke(value);
			}

			[Fact]
			public void Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = input.Audit(fSome);

				// Assert
				result.AssertSome(value);
			}
		}
	}

	public class With_FNone_And_FSome
	{
		public class With_None
		{
			[Fact]
			public void Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fNone, fSome);

				// Assert
				fNone.Received().Invoke();
			}

			[Fact]
			public void Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fNone, fSome);

				// Assert
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}

			[Fact]
			public void Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = input.Audit(fNone, fSome);

				// Assert
				result.AssertNone();
			}

			[Fact]
			public void FNone_Null_Does_Not_Throw()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = input.Audit(null, fSome);

				// Assert
				result.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public void Does_Not_Call_FNone()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fNone, fSome);

				// Assert
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public void Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = input.Audit(fNone, fSome);

				// Assert
				fSome.Received().Invoke(value);
			}

			[Fact]
			public void Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = input.Audit(fNone, fSome);

				// Assert
				result.AssertSome(value);
			}

			[Fact]
			public void FSome_Null_Does_Not_Throw()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();

				// Act
				var result = input.Audit(fNone, null);

				// Assert
				result.AssertSome(value);
			}
		}

	}

	public class With_Either
	{
		public class With_None
		{
			[Fact]
			public void Calls_Either_With_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				_ = input.Audit(either);

				// Assert
				either.Received().Invoke(input);
			}

			[Fact]
			public void Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				var result = input.Audit(either);

				// Assert
				result.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public void Calls_Either_With_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				_ = input.Audit(either);

				// Assert
				either.Received().Invoke(Arg.Is<Maybe<string>>(x => x.IsSome));
			}

			[Fact]
			public void Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				var result = input.Audit(either);

				// Assert
				result.AssertSome(value);
			}
		}

	}
}
