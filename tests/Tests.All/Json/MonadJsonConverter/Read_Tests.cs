// Wrap: Unit Tests
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
				var converter = new MonadJsonConverter<Test, string>();

				// Act
				var result = converter.Read(ref reader, typeof(Test), new());

				// Assert
				var test = Assert.IsType<Test>(result);
				Assert.Equal(value, test.Value);
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
			var converter = new MonadJsonConverter<Test, string>();

			// Act
			var result = Record.Exception(() =>
			{
				var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(input));
				return converter.Read(ref reader, typeof(Test), new());
			});

			// Assert
			Assert.IsType<NullMonadValueException>(result);
		}
	}

	public sealed record class Test() : Monad<Test, string>(Rnd.Str);
}
