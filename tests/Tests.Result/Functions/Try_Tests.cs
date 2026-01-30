// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Functions_Tests;

public class Try_Tests
{
	public class Without_Handler : Abstracts.Try_Tests.Base
	{
		[Fact]
		public override void Test00_Executes_Function() =>
			Test00((f, _) => R.Try(f));

		public new class On_Success : Abstracts.Try_Tests.On_Success
		{
			[Fact]
			public override void Test01_Returns_Ok_Result_With_Value() =>
				Test01((f, _) => R.Try(f));
		}

		public new class On_Exception : Abstracts.Try_Tests.On_Exception
		{
			public override void Test02_Executes_Handler() =>
				throw new NotImplementedException();

			[Fact]
			public override void Test03_Returns_Failure_Result_With_Exception() =>
				Test03((f, _) => R.Try(f));
		}
	}

	public class With_Handler : Abstracts.Try_Tests.Base
	{
		[Fact]
		public override void Test00_Executes_Function() =>
			Test00((f, h) => R.Try(f, h));

		public new class On_Success : Abstracts.Try_Tests.On_Success
		{
			[Fact]
			public override void Test01_Returns_Ok_Result_With_Value() =>
				Test01((f, h) => R.Try(f, h));
		}

		public new class On_Exception : Abstracts.Try_Tests.On_Exception
		{
			[Fact]
			public override void Test02_Executes_Handler() =>
				Test02((f, h) => R.Try(f, h));

			[Fact]
			public override void Test03_Returns_Failure_Result_With_Exception() =>
				Test03((f, h) => R.Try(f, h));
		}
	}
}
