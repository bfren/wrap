// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Wrap;

/// <summary>
/// Source generator for Failure objects.
/// </summary>
[Generator]
public sealed class FailureGenerator : IIncrementalGenerator
{
	// LogLevel.Error (1 << 4), the level R.Fail(Exception) uses - the generator cannot reference Wrap.Common
	private const int ExceptionLogLevel =
		1 << 4;

	/// <inheritdoc/>
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// Filter to methods decorated with [Failure]
		var methods = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				"Wrap.FailureAttribute",
				predicate: static (node, _) => node is MethodDeclarationSyntax,
				transform: static (ctx, _) => GetMethod(ctx))
			.Where(static m => m is not null);

		// Register output
		context.RegisterSourceOutput(methods, Execute);
	}

	private static MethodModel? GetMethod(GeneratorAttributeSyntaxContext ctx)
	{
		if (ctx.TargetSymbol is not IMethodSymbol method)
		{
			return null;
		}

		// Get key values from attribute
		var attr = ctx.Attributes[0];
		var message = attr.ConstructorArguments[0].Value switch
		{
			string x =>
				x,

			_ =>
				string.Empty
		};
		// Null when not set, so the Failure constructor's default level applies - or Error when there is an exception
		var logLevel = (attr.ConstructorArguments.Length > 1) switch
		{
			true when attr.ConstructorArguments[1].Value is int x =>
				x,

			_ when method.Parameters.Any(p => ParameterModel.IsExceptionType(p.Type)) =>
				ExceptionLogLevel,

			_ =>
				(int?)null
		};
		// Null when not set, so the kind stays Unspecified
		var failureKind = attr.NamedArguments.FirstOrDefault(a => a.Key == "Kind").Value.Value as string;

		// Check the method and message, and stop if the method cannot be implemented
		var location = attr.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? method.Locations.FirstOrDefault();
		var diagnostics = FailureDiagnostics.Check(method, message, failureKind, location, out var canGenerate);
		if (!canGenerate)
		{
			return new MethodModel
			{
				Diagnostics = diagnostics
			};
		}

		// Walk up ContainingType chain, collecting outermost → innermost
		var containingTypes = new Stack<ContainingType>();
		var current = method.ContainingType;
		while (current is not null)
		{
			// Record and union types are not exposed as TypeKind
			var declarations = current.DeclaringSyntaxReferences
				.Select(r => r.GetSyntax())
				.OfType<TypeDeclarationSyntax>()
				.ToArray();
			var isRecord = declarations.Any(s => s.IsKind(SyntaxKind.RecordDeclaration) || s.IsKind(SyntaxKind.RecordStructDeclaration));
			var isUnion = declarations.Any(s => s.IsKind(SyntaxKind.UnionDeclaration));

			// Get the current type
			var kind = (current.TypeKind, isRecord) switch
			{
				_ when isUnion =>
					"union",

				(TypeKind.Struct, true) =>
					"record struct",

				(TypeKind.Struct, false) =>
					"struct",

				(_, true) =>
					"record",

				(TypeKind.Interface, _) =>
					"interface",

				_ =>
					"class"
			};

			// Keep the type parameters, otherwise 'partial class Box' declares a new type instead of extending Box<T>
			var name = current.TypeParameters.Length switch
			{
				0 =>
					current.Name,

				_ =>
					$"{current.Name}<{string.Join(", ", current.TypeParameters.Select(t => t.Name))}>"
			};

			containingTypes.Push(new(name, kind));
			current = current.ContainingType; // null when we reach the namespace
		}

		// Create model
		return new MethodModel
		{
			// Empty for the global namespace, which ToDisplayString() calls "<global namespace>"
			Namespace = method.ContainingType.ContainingNamespace switch
			{
				{ IsGlobalNamespace: true } =>
					string.Empty,

				var ns =>
					ns.ToDisplayString()
			},
			ClassName = method.ContainingType.Name,
			ContainingTypes = containingTypes.ToArray(),
			Accessibility = SyntaxFacts.GetText(method.DeclaredAccessibility),
			// The documentation ID includes the parameter types, so overloads get different files
			HintName = new string([.. (method.GetDocumentationCommentId() ?? $"M:{method.Name}").Skip(2).Select(c => char.IsLetterOrDigit(c) || c == '.' ? c : '_')]) + ".g.cs",
			MethodName = method.Name,
			Message = message,
			LogLevel = logLevel,
			Kind = failureKind,
			Parameters = method.Parameters.Select(ParameterModel.Create).ToArray(),
			ReturnType = method.ReturnType.ToDisplayString(MethodModel.TypeFormat),
			Diagnostics = diagnostics,
			CanGenerate = true
		};
	}

	private static void Execute(SourceProductionContext ctx, MethodModel? model)
	{
		// Handle null
		if (model == null)
		{
			return;
		}

		// Report problems, and skip generation when the method cannot be implemented
		foreach (var diagnostic in model.Diagnostics)
		{
			ctx.ReportDiagnostic(diagnostic);
		}

		if (!model.CanGenerate)
		{
			return;
		}

		// Build lists of parameters and arguments
		// '@' before every name, so a keyword such as '@class' stays a valid name
		var paramList = string.Join(", ", model.Parameters.Select(p => $"{p.Type} @{p.Name}"));
		// Exception parameters are not message values, so they are left out of the args
		var values = model.Parameters.Where(p => !p.IsException).ToArray();
		// An explicit array: a single array parameter passed on its own would become the whole params array
		var argList = (values.Length > 0) switch
		{
			true =>
				", new object?[] { " + string.Join(", ", values.Select(p => $"@{p.Name}")) + " }",

			false =>
				""
		};

		// Build file header
		var sb = new StringBuilder()
			.AppendLine("// <auto-generated/>")
			.AppendLine("#nullable enable")
			.AppendLine()
			.Append(model.Namespace switch
			{
				"" =>
					string.Empty,

				var ns =>
					$"namespace {ns};\n\n"
			});

		// Open each containing type and add intended
		for (var i = 0; i < model.ContainingTypes.Length; i++)
		{
			var indentOpen = new string(' ', 4 * i);
			var type = model.ContainingTypes[i];
			// '@' before type and method names too, for the same reason as parameter names
			_ = sb.AppendLine($"{indentOpen}partial {type.Kind} @{type.Name}")
				.AppendLine($"{indentOpen}{{");
		}

		// Indent the method body one level per nesting depth
		var indent = new string(' ', model.ContainingTypes.Length * 4);

		// Keep the message as a template (escaped literal, not interpolated) so args are formatted later
		var message = SymbolDisplay.FormatLiteral(model.Message.TrimEnd('.') + ".", quote: true);
		// A Failure holds at most one exception (WRAP007 stops generation when there are more)
		var exception = model.Parameters.FirstOrDefault(p => p.IsException) switch
		{
			{ } p =>
				$"{indent}        Exception = @{p.Name},\n",

			null =>
				string.Empty
		};
		var level = model.LogLevel switch
		{
			int x =>
				$"{indent}        Level = (global::Wrap.Logging.LogLevel){x},\n",

			_ =>
				string.Empty
		};

		var kind = model.Kind switch
		{
			string x =>
				$"{indent}        Kind = new global::Wrap.FailureKind({SymbolDisplay.FormatLiteral(x, quote: true)}),\n",

			_ =>
				string.Empty
		};

		_ = sb.AppendLine($"{indent}{model.Accessibility} static partial {model.ReturnType} @{model.MethodName}({paramList})")
			.AppendLine($"{indent}{{")
			// no locals: a local would clash with a parameter of the same name, e.g. 'message'
			.AppendLine($"{indent}    return new {model.ReturnType}({message}{argList})")
			.AppendLine($"{indent}    {{")
			.Append(exception)
			.Append(level)
			.Append(kind)
			.AppendLine($"{indent}        Context = \"{model.ClassName}.{model.MethodName}()\"")
			.AppendLine($"{indent}    }};")
			.AppendLine($"{indent}}}");

		// Close each containing type in reverse
		for (var i = model.ContainingTypes.Length - 1; i >= 0; i--)
		{
			var indentClose = new string(' ', 4 * i);
			_ = sb.AppendLine($"{indentClose}}}");
		}

		// Add source file
		ctx.AddSource(
			model.HintName,
			SourceText.From(sb.ToString(), Encoding.UTF8)
		);
	}
}
