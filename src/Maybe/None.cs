// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

/// <summary>
/// Simple 'None' value which can be returned in place of <c>null</c>.
/// </summary>
/// <remarks>
/// <para>
/// See <see cref="M.None"/> which is where values should be created.
/// </para>
/// <para>
/// See <see cref="Maybe{T}"/> implicit operators to see how <see cref="None"/> is converted
/// to a <see cref="Maybe{T}"/>. We do this so we don't need to specify the value type when
/// returning a 'None' value.
/// </para>
/// </remarks>
public readonly struct None : IEquatable<None>
{
	/// <inheritdoc/>
	public bool Equals(None other) =>
		true;

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is None;

	/// <inheritdoc/>
	public override int GetHashCode() =>
		0;

	/// <inheritdoc/>
	public static bool operator ==(None left, None right) =>
		left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(None left, None right) =>
		!left.Equals(right);
}
