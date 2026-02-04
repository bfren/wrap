// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text;
using System.Text.Json;
using Wrap.Exceptions;

namespace Wrap.Json.MonadJsonConverter_Tests;

public class Read_Tests
{
	public class With_Valid_Json
	{
		public class With_Correct_Value_Type
		{
			[Fact]
			public void Returns_Union_With_Value()
			{
				// Arrange
				var value = Rnd.Str;
				var json = $"\"{value}\"";
				var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
				var opt = new JsonSerializerOptions();
				var converter = new MonadJsonConverter<Test, string>();

				// Act
				var result = converter.Read(ref reader, typeof(Test), opt);

				// Assert
				var test = Assert.IsType<Test>(result);
				Assert.Equal(value, test.Value);
			}
		}

		public class With_Incorrect_Value_Type
		{
			[Fact]
			public void Throws_NullMonadValueException()
			{
				// Arrange
				var value = Rnd.IntPtr;
				var json = $"{value}";
				var opt = new JsonSerializerOptions();
				var converter = new MonadJsonConverter<Test, string>();

				// Act
				Test? act()
				{
					var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
					return converter.Read(ref reader, typeof(Test), opt);
				}

				// Assert
				_ = Assert.ThrowsAny<IncorrectValueTypeException<Test, string>>(act);
			}
		}
	}

	public class With_Invalid_Json
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("null")]
		public void Throws_NullMonadValueException(string input)
		{
			// Arrange
			var opt = new JsonSerializerOptions();
			var converter = new MonadJsonConverter<Test, string>();

			// Act
			Test? act()
			{
				var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(input));
				return converter.Read(ref reader, typeof(Test), opt);
			}

			// Assert
			_ = Assert.ThrowsAny<NullMonadValueException>(act);
		}
	}

	public sealed record class Test() : Monad<Test, string>(Rnd.Str);
}
