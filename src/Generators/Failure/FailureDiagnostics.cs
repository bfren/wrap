// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Wrap;

/// <summary>
/// Compile-time checks for methods marked with [Failure].
/// </summary>
internal static class FailureDiagnostics
{
	private const string Category =
		"Wrap.Failure";

	/// <summary>
	/// The method is not <c>static partial</c> or does not return Wrap.Failure.
	/// </summary>
	internal static readonly DiagnosticDescriptor InvalidMethod =
		new(
			"WRAP001",
			"Invalid Failure method",
			"Method '{0}' must be static partial and return Wrap.Failure to use [Failure]",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	/// <summary>
	/// The message is empty.
	/// </summary>
	internal static readonly DiagnosticDescriptor EmptyMessage =
		new(
			"WRAP002",
			"Empty Failure message",
			"The [Failure] message for '{0}' must not be empty",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	/// <summary>
	/// The number of placeholders does not match the number of parameters.
	/// </summary>
	internal static readonly DiagnosticDescriptor PlaceholderCount =
		new(
			"WRAP003",
			"Placeholders do not match parameters",
			"The [Failure] message for '{0}' has {1} placeholder(s) but the method has {2} parameter(s)",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	/// <summary>
	/// A named placeholder does not match the parameter at the same position.
	/// </summary>
	internal static readonly DiagnosticDescriptor PlaceholderName =
		new(
			"WRAP004",
			"Placeholder does not match parameter",
			"Placeholder '{{{0}}}' will be filled from parameter '{1}', because placeholders are filled in parameter order",
			Category,
			DiagnosticSeverity.Warning,
			isEnabledByDefault: true
		);

	/// <summary>
	/// The method uses a feature the generator cannot implement.
	/// </summary>
	internal static readonly DiagnosticDescriptor UnsupportedMethod =
		new(
			"WRAP005",
			"Unsupported Failure method",
			"Method '{0}' cannot use [Failure] because it has {1}",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	/// <summary>
	/// The kind is set but empty.
	/// </summary>
	internal static readonly DiagnosticDescriptor EmptyKind =
		new(
			"WRAP006",
			"Empty Failure kind",
			"The [Failure] kind for '{0}' must not be empty - leave it out to use FailureKind.Unspecified",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	/// <summary>
	/// The method has more than one exception parameter, but a Failure holds only one exception.
	/// </summary>
	internal static readonly DiagnosticDescriptor MultipleExceptions =
		new(
			"WRAP007",
			"Multiple Failure exceptions",
			"Method '{0}' has more than one Exception parameter; a Failure holds one exception",
			Category,
			DiagnosticSeverity.Error,
			isEnabledByDefault: true
		);

	// Same pattern as F.Format's TemplateMatcherRegex (src/Common/Functions/F.Format.cs) - keep the two in step
	private static readonly Regex TemplateMatcher =
		new(
			@"(?<start>\{)+(?<template>[\w\.\[\]@]+)(?<format>:[^}]+)?(?<end>\})+|\{\{|\}\}",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
		);

	/// <summary>
	/// Check a [Failure] method and its message.
	/// </summary>
	/// <param name="method">Method symbol.</param>
	/// <param name="message">Failure message.</param>
	/// <param name="kind">Failure kind name, or <see langword="null"/> if not set.</param>
	/// <param name="location">Location of the attribute, used for message diagnostics.</param>
	/// <param name="canGenerate">False if the method cannot be implemented, so no source should be generated.</param>
	/// <returns>Diagnostics to report (empty if there are no problems).</returns>
	internal static Diagnostic[] Check(IMethodSymbol method, string message, string? kind, Location? location, out bool canGenerate)
	{
		// Generated code would not compile, so report and stop
		if (!method.IsStatic || !method.IsPartialDefinition || method.ReturnType.ToDisplayString() != "Wrap.Failure")
		{
			canGenerate = false;
			return [Diagnostic.Create(InvalidMethod, method.Locations.FirstOrDefault(), method.Name)];
		}

		// Generic methods would need every constraint copied exactly, and params would spread into the Failure args
		var unsupported = method switch
		{
			{ IsGenericMethod: true } =>
				"type parameters",

			_ when method.Parameters.Any(p => p.IsParams) =>
				"a params parameter",

			_ when method.Parameters.Any(p => p.RefKind != RefKind.None) =>
				"a ref, out or in parameter",

			// The generated file cannot reopen a type that only its own file can see
			_ when IsInFileLocalType(method.ContainingType) =>
				"a file-local containing type",

			_ =>
				null
		};

		if (unsupported is not null)
		{
			canGenerate = false;
			return [Diagnostic.Create(UnsupportedMethod, method.Locations.FirstOrDefault(), method.Name, unsupported)];
		}

		// A Failure holds one exception, so a second one would have nowhere to go
		var exceptions = method.Parameters.Count(p => ParameterModel.IsExceptionType(p.Type));
		if (exceptions > 1)
		{
			canGenerate = false;
			return [Diagnostic.Create(MultipleExceptions, method.Locations.FirstOrDefault(), method.Name)];
		}

		canGenerate = true;
		if (string.IsNullOrWhiteSpace(message))
		{
			return [Diagnostic.Create(EmptyMessage, location, method.Name)];
		}

		// Exception parameters are not placeholder values, so only the others are checked against the message
		var parameters = method.Parameters.Where(p => !ParameterModel.IsExceptionType(p.Type)).ToArray();

		var diagnostics = new List<Diagnostic>();
		if (kind is not null && string.IsNullOrWhiteSpace(kind))
		{
			diagnostics.Add(Diagnostic.Create(EmptyKind, location, method.Name));
		}

		// Placeholders exactly as F.Format sees them: an odd number of braces, with escaped braces ignored
		var placeholders = TemplateMatcher.Matches(message).Cast<Match>()
			.Where(m => m.Groups["template"].Success && m.Groups["start"].Captures.Count % 2 == 1)
			.Select(m => m.Groups["template"].Value.TrimStart('@'))
			.ToArray();

		// F.Format fills numbered placeholders by index and named ones in order
		var numbered = placeholders.Select(p => int.TryParse(p, out var i) ? i : (int?)null).ToArray();
		var count = numbered.All(i => i is not null) switch
		{
			true =>
				numbered.Select(i => i!.Value + 1).DefaultIfEmpty(0).Max(),

			false =>
				placeholders.Length
		};

		if (count != parameters.Length)
		{
			diagnostics.Add(Diagnostic.Create(PlaceholderCount, location, method.Name, count, parameters.Length));
		}

		// Names are only labels to F.Format, so a mismatch means the values will be in the wrong place
		for (var i = 0; i < Math.Min(placeholders.Length, parameters.Length); i++)
		{
			var name = placeholders[i].Split('.', '[')[0];
			if (numbered[i] is null && !string.Equals(name, parameters[i].Name, StringComparison.OrdinalIgnoreCase))
			{
				diagnostics.Add(Diagnostic.Create(PlaceholderName, location, placeholders[i], parameters[i].Name));
			}
		}

		return [.. diagnostics];
	}

	/// <summary>
	/// Check whether a type, or any type it is nested in, is declared <c>file</c>.
	/// </summary>
	/// <param name="type">Containing type, or <see langword="null"/> once the namespace is reached.</param>
	/// <returns><see langword="true"/> if <paramref name="type"/> or one of its containing types is file-local.</returns>
	private static bool IsInFileLocalType(INamedTypeSymbol? type) =>
		type is not null && (type.IsFileLocal || IsInFileLocalType(type.ContainingType));
}
