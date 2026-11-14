// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Wrap.Generators.FailureGenerator_Tests;

public class Diagnostics_Tests
{
	private static string[] GetDiagnosticIds(string method)
	{
		var source = $$"""
			using Wrap;
			using Wrap.Logging;

			namespace Test;

			public static partial class Failures
			{
				{{method}}
			}
			""";

		var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
			.Split(Path.PathSeparator)
			.Append(typeof(Failure).Assembly.Location)
			.Append(typeof(F).Assembly.Location)
			.Distinct()
			.Select(p => MetadataReference.CreateFromFile(p));

		var compilation = CSharpCompilation.Create(
			"Test",
			[CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken)],
			references,
			new(OutputKind.DynamicallyLinkedLibrary)
		);

		_ = CSharpGeneratorDriver.Create(new FailureGenerator())
			.RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics, TestContext.Current.CancellationToken);

		return [.. diagnostics.Select(d => d.Id)];
	}

	[Theory]
	[InlineData("""[Failure("User {UserId} was not found")] public static partial Failure F(int userId);""")]
	[InlineData("""[Failure("User {@user.Name} has {Count:N0} items", LogLevel.Error)] public static partial Failure F(object user, int count);""")]
	[InlineData("""[Failure("Values {1} and {0}")] public static partial Failure F(int a, int b);""")]
	[InlineData("""[Failure("Literal {{braces}} only")] public static partial Failure F();""")]
	public void Valid_Method__Reports_Nothing(string method)
	{
		// Arrange

		// Act
		var result = GetDiagnosticIds(method);

		// Assert
		Assert.Empty(result);
	}

	[Theory]
	[InlineData("""[Failure("Not static")] public partial Failure F();""")]
	[InlineData("""[Failure("Not partial")] public static Failure F() => default;""")]
	[InlineData("""[Failure("Wrong return type")] public static partial string F();""")]
	public void Invalid_Method__Reports_WRAP001(string method)
	{
		// Arrange

		// Act
		var result = GetDiagnosticIds(method);

		// Assert
		Assert.Equal(["WRAP001"], result);
	}

	[Theory]
	[InlineData("""[Failure("")] public static partial Failure F();""")]
	[InlineData("""[Failure("  ")] public static partial Failure F();""")]
	public void Empty_Message__Reports_WRAP002(string method)
	{
		// Arrange

		// Act
		var result = GetDiagnosticIds(method);

		// Assert
		Assert.Equal(["WRAP002"], result);
	}

	[Theory]
	[InlineData("""[Failure("User {UserId} was not found")] public static partial Failure F();""")]
	[InlineData("""[Failure("User was not found")] public static partial Failure F(int userId);""")]
	[InlineData("""[Failure("User {UserId} {Extra}")] public static partial Failure F(int userId);""")]
	[InlineData("""[Failure("Value {1}")] public static partial Failure F(int a);""")]
	[InlineData("""[Failure("Escaped {{UserId}}")] public static partial Failure F(int userId);""")]
	public void Placeholder_Count_Mismatch__Reports_WRAP003(string method)
	{
		// Arrange

		// Act
		var result = GetDiagnosticIds(method);

		// Assert
		Assert.Contains("WRAP003", result);
	}

	[Theory]
	[InlineData("""[Failure("User {UsrId} was not found")] public static partial Failure F(int userId);""", 1)]
	[InlineData("""[Failure("{B} then {A}")] public static partial Failure F(int a, int b);""", 2)]
	public void Placeholder_Name_Mismatch__Reports_WRAP004(string method, int expected)
	{
		// Arrange

		// Act
		var result = GetDiagnosticIds(method);

		// Assert
		Assert.Equal(Enumerable.Repeat("WRAP004", expected), result);
	}
}
