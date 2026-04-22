// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Holds information about a FailureValue method.
/// </summary>
internal record MethodModel()
{
	/// <summary>
	/// Containing namespace.
	/// </summary>
	internal string Namespace { get; set; } =
		string.Empty;

	/// <summary>
	/// Containing class.
	/// </summary>
	internal string ClassName { get; set; } =
		string.Empty;

	/// <summary>
	/// Type tree.
	/// </summary>
	internal ContainingType[] ContainingTypes { get; set; } =
		[];

	/// <summary>
	/// Method name.
	/// </summary>
	internal string MethodName { get; set; } =
		string.Empty;

	/// <summary>
	/// Failure message.
	/// </summary>
	internal string Message { get; set; } =
		string.Empty;

	/// <summary>
	/// Failure log level.
	/// </summary>
	internal int LogLevel { get; set; }

	/// <summary>
	/// Method parameters.
	/// </summary>
	internal ParameterModel[] Parameters { get; set; } =
		[];

	/// <summary>
	/// Method return type.
	/// </summary>
	internal string ReturnType { get; set; } =
		string.Empty;

}
