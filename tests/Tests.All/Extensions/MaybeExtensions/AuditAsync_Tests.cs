// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions.MaybeExtensions_Tests;

public class AuditAsync_Tests
{
	public class With_FNone_Only
	{
		public class With_None
		{
			[Fact]
			public async Task Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone);
				var r1 = await input.AsTask().AuditAsync(fNone);

				// Assert
				await fNone.Received(2).Invoke();
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();

				// Act
				var result = await input.AsTask().AuditAsync(fNone);

				// Assert
				fNone.Received().Invoke();
				result.AssertNone();
			}

			[Fact]
			public async Task Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone);
				var r1 = await input.AsTask().AuditAsync(fNone);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Does_Not_Call_FNone()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone);
				var r1 = await input.AsTask().AuditAsync(fNone);

				// Assert
				await fNone.DidNotReceive().Invoke();
				r0.AssertSome();
				r1.AssertSome();
			}

			[Fact]
			public async Task With_Action_Does_Not_Call_FNone()
			{
				// Arrange
				var input = M.Wrap(Rnd.Str);
				var fNone = Substitute.For<Action>();

				// Act
				_ = await input.AsTask().AuditAsync(fNone);

				// Assert
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public async Task Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone);
				var r1 = await input.AsTask().AuditAsync(fNone);

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
			}
		}
	}

	public class With_FSome_Only
	{
		public class With_None
		{
			[Fact]
			public async Task Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fSome);
				var r1 = await input.AsTask().AuditAsync(fSome);

				// Assert
				await fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fSome);

				// Assert
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}

			[Fact]
			public async Task Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fSome);
				var r1 = await input.AsTask().AuditAsync(fSome);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fSome);
				var r1 = await input.AsTask().AuditAsync(fSome);

				// Assert
				await fSome.Received(2).Invoke(value);
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task With_Action_Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fSome);

				// Assert
				fSome.Received().Invoke(value);
			}

			[Fact]
			public async Task Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fSome);
				var r1 = await input.AsTask().AuditAsync(fSome);

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
			}
		}
	}

	public class With_FNone_And_FSome
	{
		public class With_None
		{
			[Fact]
			public async Task Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				await fNone.Received(2).Invoke();
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_Calls_FNone()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				fNone.Received().Invoke();
			}

			[Fact]
			public async Task Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				await fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_Does_Not_Call_FSome()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				fSome.DidNotReceiveWithAnyArgs().Invoke(Arg.Any<string>());
			}

			[Fact]
			public async Task Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task FNone_Null_Does_Not_Throw()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(null, fSome);
				var r1 = await input.AsTask().AuditAsync(null, fSome);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_FNone_Null_Does_Not_Throw()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				var result = await input.AsTask().AuditAsync(null, fSome);

				// Assert
				result.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Does_Not_Call_FNone()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				await fNone.DidNotReceive().Invoke();
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task With_Action_Does_Not_Call_FNone()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				fNone.DidNotReceive().Invoke();
			}

			[Fact]
			public async Task Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				await fSome.Received(2).Invoke(value);
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task With_Action_Calls_FSome_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();
				var fSome = Substitute.For<Action<string>>();

				// Act
				_ = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				fSome.Received().Invoke(value);
			}

			[Fact]
			public async Task Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);
				var fSome = Substitute.For<Func<string, Task>>();
				fSome.Invoke(Arg.Any<string>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, fSome);
				var r1 = await input.AsTask().AuditAsync(fNone, fSome);

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task FSome_Null_Does_Not_Throw()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Func<Task>>();
				fNone.Invoke().Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(fNone, null);
				var r1 = await input.AsTask().AuditAsync(fNone, null);

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task With_Action_FSome_Null_Does_Not_Throw()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var fNone = Substitute.For<Action>();

				// Act
				var result = await input.AsTask().AuditAsync(fNone, null);

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
			public async Task Calls_Either_With_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var either = Substitute.For<Func<Maybe<string>, Task>>();
				either.Invoke(Arg.Any<Maybe<string>>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(either);
				var r1 = await input.AsTask().AuditAsync(either);

				// Assert
				await either.Received(2).Invoke(input);
				r0.AssertNone();
				r1.AssertNone();
			}

			[Fact]
			public async Task With_Action_Calls_Either_With_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				_ = await input.AsTask().AuditAsync(either);

				// Assert
				either.Received().Invoke(input);
			}

			[Fact]
			public async Task Returns_Original_None()
			{
				// Arrange
				var input = NoneGen.Create<string>();
				var either = Substitute.For<Func<Maybe<string>, Task>>();
				either.Invoke(Arg.Any<Maybe<string>>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(either);
				var r1 = await input.AsTask().AuditAsync(either);

				// Assert
				r0.AssertNone();
				r1.AssertNone();
			}
		}

		public class With_Some
		{
			[Fact]
			public async Task Calls_Either_With_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var either = Substitute.For<Func<Maybe<string>, Task>>();
				either.Invoke(Arg.Any<Maybe<string>>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(either);
				var r1 = await input.AsTask().AuditAsync(either);

				// Assert
				await either.Received(2).Invoke(Arg.Is<Maybe<string>>(x => x.IsSome));
				r0.AssertSome(value);
				r1.AssertSome(value);
			}

			[Fact]
			public async Task With_Action_Calls_Either_With_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var either = Substitute.For<Action<Maybe<string>>>();

				// Act
				_ = await input.AsTask().AuditAsync(either);

				// Assert
				either.Received().Invoke(Arg.Is<Maybe<string>>(x => x.IsSome));
			}

			[Fact]
			public async Task Returns_Original_Some()
			{
				// Arrange
				var value = Rnd.Str;
				var input = M.Wrap(value);
				var either = Substitute.For<Func<Maybe<string>, Task>>();
				either.Invoke(Arg.Any<Maybe<string>>()).Returns(Task.CompletedTask);

				// Act
				var r0 = await input.AuditAsync(either);
				var r1 = await input.AsTask().AuditAsync(either);

				// Assert
				r0.AssertSome(value);
				r1.AssertSome(value);
			}
		}

	}
}
