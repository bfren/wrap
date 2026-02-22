// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class TryAsync_Tests
{
	public class Without_Handler : Abstracts.Try_Tests.Base_Async
	{
		[Fact]
		public override Task Test00_Executes_Function() =>
			Test00_Async((f, _) => R.TryAsync(f));

		public new class On_Success : On_Success_Async
		{
			[Fact]
			public override Task Test01_Returns_Ok_Result_With_Value() =>
				Test01_Async((f, _) => R.TryAsync(f));
		}

		public new class On_Exception : On_Exception_Async
		{
			public override Task Test02_Executes_Handler() =>
				throw new NotImplementedException();

			[Fact]
			public override Task Test03_Returns_Failure_Result_With_Exception() =>
				Test03_Async((f, _) => R.TryAsync(f));
		}
	}

	public class With_Handler : Abstracts.Try_Tests.Base_Async
	{
		[Fact]
		public override Task Test00_Executes_Function() =>
			Test00_Async((f, h) => R.TryAsync(f, h));

		public new class On_Success : On_Success_Async
		{
			[Fact]
			public override Task Test01_Returns_Ok_Result_With_Value() =>
				Test01_Async((f, h) => R.TryAsync(f, h));
		}

		public new class On_Exception : On_Exception_Async
		{
			[Fact]
			public override Task Test02_Executes_Handler() =>
				Test02_Async((f, h) => R.TryAsync(f, h));

			[Fact]
			public override Task Test03_Returns_Failure_Result_With_Exception() =>
				Test03_Async((f, h) => R.TryAsync(f, h));
		}
	}
}
