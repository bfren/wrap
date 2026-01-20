// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// Generid ID using <see cref="long"/> as the Value type.
/// </summary>
public interface ILongId : IUnion
{
	/// <summary>
	/// ID Value.
	/// </summary>
	new long Value { get; }
}
