// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.F_Tests;

/// <summary>
/// One row per input form F.Format accepts, so a change to any one form shows its effect on all the others.
/// Expected values follow string.Format for numbered placeholders and Serilog message templates for named ones.
/// Mixed numbered and named placeholders, and alignment (e.g. {0,3}), are not supported so have no rows.
/// </summary>
public class Format_Forms_Tests
{
	[Theory]
	// numbered - matched by index
	[InlineData("{0} and {1}", "a and b", new object[] { "a", "b" })]
	[InlineData("{1} and {0}", "b and a", new object[] { "a", "b" })]
	[InlineData("{0} and {0}", "a and a", new object[] { "a", "b" })]
	[InlineData("{0} and {5}", "a and {5}", new object[] { "a" })]
	[InlineData("n={0:00}", "n=05", new object[] { 5 })]
	// named - matched by position
	[InlineData("{A} and {B}", "a and b", new object[] { "a", "b" })]
	[InlineData("{@A} and {B}", "a and b", new object[] { "a", "b" })]
	[InlineData("{A} and {B}", "a and {B}", new object[] { "a" })]
	[InlineData("n={A:00}", "n=05", new object[] { 5 })]
	// escaped braces print one brace and take no argument
	[InlineData("{{0}}", "{0}", new object[] { 42 })]
	[InlineData("{{literal}} and {A}", "{literal} and 42", new object[] { 42 })]
	[InlineData("{{{A}}}", "{42}", new object[] { 42 })]
	// braces that are not placeholders are printed as they are
	[InlineData("rule {max=5} for {A}", "rule {max=5} for 42", new object[] { 42 })]
	[InlineData("rule {max=5} for {0}", "rule {max=5} for 42", new object[] { 42 })]
	[InlineData("a { b {A}", "a { b 42", new object[] { 42 })]
	[InlineData("a } b {A}", "a } b 42", new object[] { 42 })]
	[InlineData("{{ and }} around {A}", "{ and } around 42", new object[] { 42 })]
	// unsupported or invalid input returns the format string unchanged
	[InlineData("{A,3}|", "{A,3}|", new object[] { 5 })]
	[InlineData("n={A:Q}", "n={A:Q}", new object[] { 5 })]
	[InlineData("{{{A}} and {B}", "{{{A}} and {B}", new object[] { 42, 43 })]
	public void Array_Source(string format, string expected, object[] args)
	{
		// Arrange

		// Act
		var result = F.Format(format, args);

		// Assert
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData("{Name} has {Count}", "n has 5")]
	[InlineData("{name} has {count}", "n has 5")]
	[InlineData("n={Count:000}", "n=005")]
	[InlineData("{Missing} and {Name}", "{Missing} and n")]
	[InlineData("{0}", "thing")]
	[InlineData("{{Name}} is {Name}", "{Name} is n")]
	[InlineData("{a=1} {Name}", "{a=1} n")]
	public void Object_Source(string format, string expected)
	{
		// Arrange
		var source = new Thing("n", 5);

		// Act
		var result = F.Format(format, source);

		// Assert
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData("{0}/", "42/")]
	[InlineData("{0} {1}", "42 {1}")]
	[InlineData("{bar}/", "{bar}/")]
	[InlineData("{{0}} {0}", "{0} 42")]
	public void Single_Value_Source(string format, string expected)
	{
		// Arrange

		// Act
		var result = F.Format(format, 42);

		// Assert
		Assert.Equal(expected, result);
	}

	public sealed record class Thing(string Name, int Count)
	{
		public override string ToString() =>
			"thing";
	}
}
