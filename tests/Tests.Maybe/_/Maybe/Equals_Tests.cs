// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Exceptions;

namespace Wrap.Maybe_Tests;

public class Equals_Tests
{
	public class Some
	{
		public class With_Some
		{
			[Fact]
			public void Same_Value_Returns_True()
			{
				// Arrange
				var value = Rnd.Guid;
				var v0 = M.Wrap(value);
				var v1 = M.Wrap(value);

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.True(r0);
				Assert.True(r1);
				Assert.False(r2);
			}

			[Fact]
			public void Different_Value_Returns_False()
			{
				// Arrange
				var v0 = M.Wrap(Rnd.Guid);
				var v1 = M.Wrap(Rnd.Guid);

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}

		public class With_None
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var v0 = M.Wrap(Rnd.Str);
				var v1 = NoneGen.Create<string>();

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}

		public class With_Invalid_Type
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var v0 = M.Wrap(Rnd.Date);
				var v1 = new InvalidMaybe<DateOnly>();

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}

		public class With_Value
		{
			[Fact]
			public void Same_Value_Returns_True()
			{
				// Arrange
				var value = Rnd.Str;
				var wrapped = M.Wrap(value);

				// Act
				var r0 = value == wrapped;
				var r1 = wrapped == value;
				var r2 = value != wrapped;
				var r3 = wrapped != value;

				// Assert
				Assert.True(r0);
				Assert.True(r1);
				Assert.False(r2);
				Assert.False(r3);
			}

			[Fact]
			public void Different_Value_Returns_False()
			{
				// Arrange
				var value = Rnd.Str;
				var wrapped = M.Wrap(Rnd.Str);

				// Act
				var r0 = value == wrapped;
				var r1 = wrapped == value;
				var r2 = value != wrapped;
				var r3 = wrapped != value;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
				Assert.True(r3);
			}
		}

		public class With_Null
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var value = M.Wrap(Rnd.Lng);

				// Act
				var r0 = value.Equals(null);
				var r1 = value == null;
				var r2 = value != null;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}
	}

	public class None
	{
		public class With_None
		{
			[Fact]
			public void Returns_True()
			{
				// Arrange
				var v0 = NoneGen.Create<string>();
				var v1 = NoneGen.Create<string>();

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.True(r0);
				Assert.True(r1);
				Assert.False(r2);
			}
		}

		public class With_Invalid_Type
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var v0 = NoneGen.Create<DateOnly>();
				var v1 = new InvalidMaybe<DateOnly>();

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}

		public class With_Value
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var value = Rnd.Str;
				var wrapped = NoneGen.Create<string>();

				// Act
				var r0 = value == wrapped;
				var r1 = wrapped == value;
				var r2 = value != wrapped;
				var r3 = wrapped != value;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
				Assert.True(r3);
			}
		}

		public class With_Null
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var value = NoneGen.Create<DateOnly>();

				// Act
				var r0 = value.Equals(null);
				var r1 = value == null;
				var r2 = value != null;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}
	}

	public class Invalid_Type
	{
		public class With_None
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var v0 = new InvalidMaybe<string>();
				var v1 = NoneGen.Create<string>();

				// Act
				var r0 = v0.Equals(v1);
				var r1 = v0 == v1;
				var r2 = v0 != v1;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}

		public class With_Value
		{
			[Fact]
			public void Throws_InvalidMaybeTypeException()
			{
				// Arrange
				var value = Rnd.Str;
				var wrapped = new InvalidMaybe<string>();

				// Act
				var r0 = Record.Exception(() => value == wrapped);
				var r1 = Record.Exception(() => wrapped == value);
				var r2 = Record.Exception(() => value != wrapped);
				var r3 = Record.Exception(() => wrapped != value);

				// Assert
				_ = Assert.IsType<InvalidMaybeTypeException>(r0);
				_ = Assert.IsType<InvalidMaybeTypeException>(r1);
				_ = Assert.IsType<InvalidMaybeTypeException>(r2);
				_ = Assert.IsType<InvalidMaybeTypeException>(r3);
			}
		}

		public class With_Null
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var value = new InvalidMaybe<string>();

				// Act
				var r0 = value.Equals(null);
				var r1 = value == null;
				var r2 = value != null;

				// Assert
				Assert.False(r0);
				Assert.False(r1);
				Assert.True(r2);
			}
		}
	}
}
