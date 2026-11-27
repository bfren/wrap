// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;
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
	/// Gets whether this parameter is the failure's exception, rather than a value for its message.
	/// </summary>
	internal bool IsException { get; set; }

	/// <summary>
	/// Create from <see cref="IParameterSymbol"/>.
	/// </summary>
	/// <param name="symbol">IParameterSymbol.</param>
	/// <returns>ParameterModel.</returns>
	internal static ParameterModel Create(IParameterSymbol symbol) =>
		new()
		{
			Type = symbol.Type.ToDisplayString(MethodModel.TypeFormat),
			Name = symbol.Name,
			IsException = IsExceptionType(symbol.Type)
		};

	/// <summary>
	/// Check whether a type is <see cref="Exception"/> or derives from it, or is a type parameter constrained to one.
	/// </summary>
	/// <param name="type">Parameter type.</param>
	/// <returns><see langword="true"/> if the type is, or is constrained to, <see cref="Exception"/>.</returns>
	internal static bool IsExceptionType(ITypeSymbol type) =>
		type switch
		{
			ITypeParameterSymbol parameter =>
				parameter.ConstraintTypes.Any(IsExceptionType),

			_ =>
				IsExceptionClass(type)
		};

	/// <summary>
	/// Walk the base types of <paramref name="type"/> to check for <c>System.Exception</c>.
	/// </summary>
	/// <param name="type">Type to check.</param>
	/// <returns><see langword="true"/> if <paramref name="type"/> is or derives from <c>System.Exception</c>.</returns>
	private static bool IsExceptionClass(ITypeSymbol? type) =>
		type is not null && (type.Name == nameof(Exception) && type.ContainingNamespace?.ToDisplayString() == "System"
			|| IsExceptionClass(type.BaseType));
}
