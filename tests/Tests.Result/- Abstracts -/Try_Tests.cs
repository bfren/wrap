// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using NSubstitute.ExceptionExtensions;

namespace Abstracts;

public abstract class Try_Tests
{
	public abstract class Base : Try_Tests
	{
		public abstract void Test00_Executes_Function();
	}

	public abstract class Base_Async : Try_Tests
	{
		public abstract Task Test00_Executes_Function();
	}

	public abstract class On_Success : Try_Tests
	{
		public abstract void Test01_Returns_Ok_Result_With_Value();
	}

	public abstract class On_Success_Async : Try_Tests
	{
		public abstract Task Test01_Returns_Ok_Result_With_Value();
	}

	public abstract class On_Exception : Try_Tests
	{
		public abstract void Test02_Executes_Handler();

		public abstract void Test03_Returns_Failure_Result_With_Exception();
	}

	public abstract class On_Exception_Async : Try_Tests
	{
		public abstract Task Test02_Executes_Handler();

		public abstract Task Test03_Returns_Failure_Result_With_Exception();
	}

	protected void Test00(Action<Func<int>, R.ExceptionHandler> act)
	{
		// Arrange
		var f = Substitute.For<Func<int>>();
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		act(f, handler);

		// Assert
		f.Received().Invoke();
	}

	protected async Task Test00_Async(Func<Func<Task<int>>, R.ExceptionHandler, Task> act)
	{
		// Arrange
		var f = Substitute.For<Func<Task<int>>>();
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		await act(f, handler);

		// Assert
		await f.Received().Invoke();
	}

	protected void Test01(Func<Func<Guid>, R.ExceptionHandler, Result<Guid>> act)
	{
		// Arrange
		var value = Rnd.Guid;
		var f = Substitute.For<Func<Guid>>();
		f.Invoke().Returns(value);
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		var result = act(f, handler);

		// Assert
		result.AssertOk(value);
	}

	protected async Task Test01_Async(Func<Func<Task<Guid>>, R.ExceptionHandler, Task<Result<Guid>>> act)
	{
		// Arrange
		var value = Rnd.Guid;
		var f = Substitute.For<Func<Task<Guid>>>();
		f.Invoke().Returns(value);
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		var result = await act(f, handler);

		// Assert
		result.AssertOk(value);
	}

	protected void Test02(Action<Func<string>, R.ExceptionHandler> act)
	{
		// Arrange
		var f = Substitute.For<Func<string>>();
		var ex = new Exception(Rnd.Str);
		f.Invoke().Throws(ex);
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		act(f, handler);

		// Assert
		handler.Received(1).Invoke(ex);
	}

	protected async Task Test02_Async(Func<Func<Task<string>>, R.ExceptionHandler, Task> act)
	{
		// Arrange
		var f = Substitute.For<Func<Task<string>>>();
		var ex = new Exception(Rnd.Str);
		f.Invoke().ThrowsAsync(ex);
		var handler = Substitute.For<R.ExceptionHandler>();

		// Act
		await act(f, handler);

		// Assert
		handler.Received(1).Invoke(ex);
	}

	protected void Test03(Func<Func<int>, R.ExceptionHandler, Result<int>> act)
	{
		// Arrange
		var f = Substitute.For<Func<int>>();
		var ex = new Exception(Rnd.Str);
		f.Invoke().Throws(ex);
		var handler = Substitute.For<R.ExceptionHandler>();
		handler.Invoke(ex).Returns(R.Fail(ex));

		// Act
		var result = act(f, handler);

		// Assert
		_ = result.AssertFailure(ex);
	}

	protected async Task Test03_Async(Func<Func<Task<int>>, R.ExceptionHandler, Task<Result<int>>> act)
	{
		// Arrange
		var f = Substitute.For<Func<Task<int>>>();
		var ex = new Exception(Rnd.Str);
		f.Invoke().ThrowsAsync(ex);
		var handler = Substitute.For<R.ExceptionHandler>();
		handler.Invoke(ex).Returns(R.Fail(ex));

		// Act
		var result = await act(f, handler);

		// Assert
		_ = result.AssertFailure(ex);
	}
}
