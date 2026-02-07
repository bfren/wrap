// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json.MonadJsonConverter_Tests;

public class Write_Tests
{
	public class With_Null
	{
		[Fact]
		public void Writes_Null_Values()
		{
			// Arrange
			var obj = new TestObj(null!, null!, null!, null!);
			var opt = new JsonSerializerOptions();
			opt.AddMonadConverter();

			// Act
			var result = JsonSerializer.Serialize(obj, opt);

			// Assert
			Assert.Equal(/*lang=json,strict*/ "{\"Foo\":null,\"Bar\":null,\"Gu\":null,\"When\":null}", result);
		}
	}

	public class With_Value
	{
		[Fact]
		public void Writes_Json_Values()
		{
			// Arrange
			var intVal = Rnd.Int;
			var strVal = Rnd.Str;
			var guidVal = Rnd.Guid;
			var dtVal = Rnd.Date;
			var obj = new TestObj(
				TestInt.Wrap(intVal),
				TestStr.Wrap(strVal),
				TestGuid.Wrap(guidVal),
				TestDate.Wrap(dtVal)
			);
			var opt = new JsonSerializerOptions();
			opt.AddMonadConverter();

			// Act
			var result = JsonSerializer.Serialize(obj, opt);

			// Assert
			Assert.Equal($"{{\"Foo\":{intVal},\"Bar\":\"{strVal}\",\"Gu\":\"{guidVal}\",\"When\":\"{dtVal:o}\"}}", result);
		}
	}

	public sealed record class TestInt : Monad<TestInt, int>;
	public sealed record class TestStr : Monad<TestStr, string>;
	public sealed record class TestGuid : Monad<TestGuid, Guid>;
	public sealed record class TestDate : Monad<TestDate, DateOnly>;

	public sealed record class TestObj(TestInt Foo, TestStr Bar, TestGuid Gu, TestDate When);
}
