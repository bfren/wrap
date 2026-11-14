// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

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
				var v1 = default(Maybe<string>);

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
	}

	public class None
	{
		public class With_None
		{
			[Fact]
			public void Returns_True()
			{
				// Arrange
				var v0 = default(Maybe<string>);
				var v1 = default(Maybe<string>);

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

		public class With_Value
		{
			[Fact]
			public void Returns_False()
			{
				// Arrange
				var value = Rnd.Str;
				var wrapped = default(Maybe<string>);

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
	}
}
