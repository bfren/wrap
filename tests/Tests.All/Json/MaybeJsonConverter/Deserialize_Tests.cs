// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;

namespace Wrap.Json.MaybeJsonConverter_Tests;

public class Deserialize_Tests
{
	public class Direct
	{
		public class With_Value_Type
		{
			public class With_Valid_Json
			{
				[Fact]
				public void Deserialises_Maybe()
				{
					// Arrange
					var value = Rnd.Guid;
					var json = $"\"{value}\"";

					// Act
					var result = JsonSerializer.Deserialize<Maybe<Guid>>(json, Helpers.Json.Options);

					// Assert
					result!.AssertSome(value);
				}
			}

			public class With_Invalid_Json
			{
				[Theory]
				[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
				public void Returns_Default_Value(string input)
				{
					// Arrange

					// Act
					var result = JsonSerializer.Deserialize<Maybe<Guid>>(input, Helpers.Json.Options);

					// Assert
					result!.AssertSome(default);
				}
			}
		}

		public class With_Ref_Type
		{
			public class With_Valid_Json
			{
				[Fact]
				public void Deserialises_Maybe()
				{
					// Arrange
					var valueStr = Rnd.Str;
					var valueInt = Rnd.Int;
					var json = $"{{\"foo\":\"{valueStr}\",\"bar\":{valueInt}}}";
					var expected = new Test(valueStr, valueInt);

					// Act
					var result = JsonSerializer.Deserialize<Maybe<Test>>(json, Helpers.Json.Options);

					// Assert
					result!.AssertSome(expected);
				}
			}

			public class With_Invalid_Json
			{
				[Theory]
				[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
				public void Throws_JsonException(string input)
				{
					// Arrange
					var json = $"{{\"test\":{input}}}";

					// Act
					var result = Record.Exception(() => JsonSerializer.Deserialize<RefWrapper>(json, Helpers.Json.Options));

					// Assert
					Assert.IsType<JsonException>(result);
				}
			}
		}
	}

	public class Wrapped
	{
		public class With_Value_Type
		{
			public class With_Valid_Json
			{
				[Fact]
				public void Deserialises_Maybe()
				{
					// Arrange
					var value = Rnd.Guid;
					var json = $"{{\"test\":\"{value}\"}}";
					var expected = new ValWrapper(M.Wrap(value));

					// Act
					var result = JsonSerializer.Deserialize<ValWrapper>(json, Helpers.Json.Options);

					// Assert
					Assert.Equal(expected, result);
				}
			}

			public class With_Invalid_Json
			{
				[Theory]
				[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
				public void Returns_Default_Value(string input)
				{
					// Arrange
					var json = $"{{\"test\":{input}}}";

					// Act
					var result = JsonSerializer.Deserialize<ValWrapper>(json, Helpers.Json.Options);

					// Assert
					result!.Test.AssertSome(default);
				}
			}
		}

		public class With_Ref_Type
		{
			public class With_Valid_Json
			{
				[Fact]
				public void Deserialises_Maybe()
				{
					// Arrange
					var valueStr = Rnd.Str;
					var valueInt = Rnd.Int;
					var json = $"{{\"test\":{{\"foo\":\"{valueStr}\",\"bar\":{valueInt}}}}}";
					var expected = new RefWrapper(new Test(valueStr, valueInt));

					// Act
					var result = JsonSerializer.Deserialize<RefWrapper>(json, Helpers.Json.Options);

					// Assert
					Assert.Equal(expected, result);
				}
			}

			public class With_Invalid_Json
			{

				[Theory]
				[MemberData(nameof(Helpers.Json.Invalid_Json_Data), MemberType = typeof(Helpers.Json))]
				public void Returns_Default_Value(string input)
				{
					// Arrange
					var json = $"{{\"test\":{input}}}";

					// Act
					var result = JsonSerializer.Deserialize<ValWrapper>(json, Helpers.Json.Options);

					// Assert
					result!.Test.AssertSome(default);
				}
			}
		}
	}

	public record class Test(string Foo, int Bar);

	public record class RefWrapper(Maybe<Test> Test);

	public record class ValWrapper(Maybe<Guid> Test);
}
