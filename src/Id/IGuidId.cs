// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Generid ID using <see cref="Guid"/> as the Value type.
/// </summary>
public interface IGuidId : IUnion
{
	/// <summary>
	/// ID Value.
	/// </summary>
	new Guid Value { get; }
}
