// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Generid ID using <see cref="uint"/> as the Value type.
/// </summary>
public interface IUintId : IUnion
{
	/// <summary>
	/// ID Value.
	/// </summary>
	new uint Value { get; }
}
