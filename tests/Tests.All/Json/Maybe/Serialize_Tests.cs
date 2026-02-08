// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json.Maybe_Tests;

public class Serialize_Tests
{
	public class Direct
	{
		public class With_Value_Type
		{
			[Fact]
			public void Returns_Valid_Json()
			{
				// Arrange
				var value = Rnd.Guid;
				var expected = JsonSerializer.Serialize(value);

				// Act
				var result = JsonSerializer.Serialize(M.Wrap(value), Helpers.Json.Options);

				// Assert
				Assert.Equal(expected, result);
			}
		}

		public class With_Ref_Type
		{
			[Fact]
			public void Returns_Valid_Json()
			{
				// Arrange
				var intValue = Rnd.Int;
				var strValue = Rnd.Str;
				var test = new Test(strValue, intValue);
				var expected = JsonSerializer.Serialize(test);

				// Act
				var result = JsonSerializer.Serialize(M.Wrap(test), Helpers.Json.Options);

				// Assert
				Assert.Equal(expected, result);
			}
		}
	}

	public class Wrapped
	{
		public class With_Value_Type
		{
			[Fact]
			public void Returns_Valid_Json()
			{
				// Arrange
				var value = Rnd.Guid;
				var test = new ValWrapper(value);
				var expected = $"{{\"Test\":\"{value}\"}}";

				// Act
				var result = JsonSerializer.Serialize(test, Helpers.Json.Options);

				// Assert
				Assert.Equal(expected, result);
			}
		}

		public class With_Ref_Type
		{
			[Fact]
			public void Returns_Valid_Json()
			{
				// Arrange
				var intValue = Rnd.Int;
				var strValue = Rnd.Str;
				var test = new Test(strValue, intValue);
				var expected = $"{{\"Foo\":\"{strValue}\",\"Bar\":{intValue}}}";

				// Act
				var result = JsonSerializer.Serialize(test, Helpers.Json.Options);

				// Assert
				Assert.Equal(expected, result);
			}
		}
	}

	public record class Test(string Foo, int Bar);

	public record class RefWrapper(Maybe<Test> Test);

	public record class ValWrapper(Maybe<Guid> Test);
}
