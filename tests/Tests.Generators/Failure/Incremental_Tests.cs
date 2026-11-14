// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Wrap.Generators.FailureGenerator_Tests;

public class Incremental_Tests
{
	[Fact]
	public void Unrelated_Edit__Reuses_Cached_Output()
	{
		// Arrange
		var compilation = Diagnostics_Tests.CreateCompilation(
			"""[Failure("User {UserId} was not found")] public static partial Failure F(int userId);"""
		);
		var driver = CSharpGeneratorDriver.Create(
			[new FailureGenerator().AsSourceGenerator()],
			driverOptions: new(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true)
		).RunGenerators(compilation, TestContext.Current.CancellationToken);
		var edited = compilation.AddSyntaxTrees(
			CSharpSyntaxTree.ParseText("namespace Test; public class Other { }", cancellationToken: TestContext.Current.CancellationToken)
		);

		// Act
		var result = driver.RunGenerators(edited, TestContext.Current.CancellationToken).GetRunResult().Results[0];

		// Assert
		var reasons = result.TrackedOutputSteps.SelectMany(x => x.Value).SelectMany(x => x.Outputs).Select(x => x.Reason);
		Assert.NotEmpty(reasons);
		Assert.All(reasons, x => Assert.True(x is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged, $"Output was {x}."));
	}
}
