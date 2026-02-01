// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.StringExtensions_Tests;

public class Format_Tests
{
	public class With_Invalid_Format_String
	{
		[Theory]
		[InlineData(null!)]
		[InlineData("")]
		[InlineData(" ")]
		public void Returns_Empty_String(string? input)
		{
			// Arrange

			// Act
			var result = F.Format(input!, Rnd.Int);

			// Assert
			Assert.Empty(result);
		}

		[Theory]
		[InlineData(null!)]
		[InlineData("")]
		[InlineData(" ")]
		public void Returns_Replace(string? input)
		{
			// Arrange
			var value = Rnd.Str;

			// Act
			var result = F.Format(input!, Rnd.Int, value);

			// Assert
			Assert.Equal(value, result);
		}
	}

	public class With_Numbered_Placeholders
	{
		public class With_Single_Value_Source
		{
			[Fact]
			public void Replaces_With_Value()
			{
				// Arrange
				var format = "{0}/";
				var value = Rnd.Guid;

				// Act
				var result = F.Format(format, value, string.Empty);

				// Assert
				Assert.Equal($"{value}/", result);
			}
		}

		public class With_Object_Source
		{
			[Fact]
			public void Returns_Format()
			{
				// Arrange
				const string format = "{0} , {1} , {2}";
				var values = new { zero = 3, one = 4, two = 5 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal(format, result);
			}
		}

		public class With_Array_Source
		{
			[Fact]
			public void Replaces_With_Ordered_Values()
			{
				// Arrange
				const string format = "{0} , {1} , {2}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0} , {v1} , {v2}", result);
			}

			[Fact]
			public void Replaces_With_Unordered_Values()
			{
				// Arrange
				const string format = "{1} , {0} , {2}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v1} , {v0} , {v2}", result);
			}

			[Fact]
			public void Replaces_Multiple_Values()
			{
				// Arrange
				const string format = "{1} , {0} , {2} , {0}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v1} , {v0} , {v2} , {v0}", result);
			}

			[Fact]
			public void Replaces_With_Formatted_Values()
			{
				// Arrange
				const string format = "{0:00} , {1:0.00} , {2:0,000.0}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0:00} , {v1:0.00} , {v2:0,000.0}", result);
			}
		}
	}

	public class With_Named_Placeholders
	{
		public class With_Single_Value_Source
		{
			[Fact]
			public void Returns_Format()
			{
				// Arrange
				var format = "{bar}/";

				// Act
				var result = F.Format(format, Rnd.Lng, string.Empty);

				// Assert
				Assert.Equal(format, result);
			}
		}

		public class With_Object_Source
		{
			[Fact]
			public void Replaces_With_Values()
			{
				// Arrange
				const string format = "{zero} , {@one} , {two}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new { one = v1, two = v2, zero = v0 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0} , {v1} , {v2}", result);
			}

			[Fact]
			public void Replaces_With_Formatted_Values()
			{
				// Arrange
				const string format = "{zero:00} , {one:0.00} , {two:0,000.0}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new { one = v1, two = v2, zero = v0 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0:00} , {v1:0.00} , {v2:0,000.0}", result);
			}

			[Fact]
			public void Replaces_Multiple_Values()
			{
				// Arrange
				const string format = "{two} , {zero} , {@one} , {two} , {one}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new { one = v1, two = v2, zero = v0 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v2} , {v0} , {v1} , {v2} , {v1}", result);
			}
		}

		public class With_Array_Source
		{
			[Fact]
			public void Replaces_With_Values()
			{
				// Arrange
				const string format = "{zero} , {one} , {two:0.0}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Str;
				var v2 = (double)Rnd.Int;
				var values = new object[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0} , {v1} , {v2:0.0}", result);
			}

			[Fact]
			public void Replaces_With_Formatted_Values()
			{
				// Arrange
				const string format = "{zero:00} , {one:0.00} , {2:0,000.0}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new object[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0:00} , {v1:0.00} , {v2:0,000.0}", result);
			}
		}
	}

	public class With_Mixed_Placeholders
	{
		public class With_Object_Source
		{
			[Fact]
			public void Returns_Format()
			{
				// Arrange
				const string format = "{zero} , {0} , {1}";
				var values = new { zero = Rnd.Int, one = Rnd.Int, two = Rnd.Int };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal(format, result);
			}
		}

		public class With_Array_Source
		{
			[Fact]
			public void Replaces_With_Values()
			{
				// Arrange
				const string format = "{zero} , {0} , {1}";
				var v0 = Rnd.Int;
				var v1 = Rnd.Int;
				var v2 = Rnd.Int;
				var values = new[] { v0, v1, v2 };

				// Act
				var result = F.Format(format, values);

				// Assert
				Assert.Equal($"{v0} , {v1} , {v2}", result);
			}
		}
	}
}
