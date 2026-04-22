// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.CodeAnalysis;

namespace Wrap;

/// <summary>
/// Represents a method parameter.
/// </summary>
internal record ParameterModel()
{
	/// <summary>
	/// Parameter type.
	/// </summary>
	internal string Type { get; set; } =
		string.Empty;

	/// <summary>
	/// Parameter name
	/// </summary>
	internal string Name { get; set; } =
		string.Empty;

	/// <summary>
	/// Create from <see cref="IParameterSymbol"/>.
	/// </summary>
	/// <param name="symbol">IParameterSymbol.</param>
	/// <returns>ParameterModel.</returns>
	internal static ParameterModel Create(IParameterSymbol symbol) =>
		new()
		{
			Type = symbol.Type.ToDisplayString(),
			Name = symbol.Name
		};
}
