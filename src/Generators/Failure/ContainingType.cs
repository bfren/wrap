// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Used to build the file name and source tree.
/// </summary>
internal record ContainingType
{
	/// <summary>
	/// Type name.
	/// </summary>
	internal string Name { get; set; } =
		string.Empty;

	/// <summary>
	/// Type kind (e.g. 'class').
	/// </summary>
	internal string Kind { get; set; } =
		string.Empty;

	/// <summary>
	/// Create object.
	/// </summary>
	/// <param name="name">Containing Type name.</param>
	/// <param name="kind">Containint Type kind (e.g. 'class').</param>
	internal ContainingType(string name, string kind) =>
		(Name, Kind) = (name, kind);
}
