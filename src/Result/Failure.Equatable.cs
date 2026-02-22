// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public readonly partial struct Failure
{
	/// <inheritdoc/>
	public bool Equals(Failure other) =>
		Value.Equals(other.Value);

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is Failure failure && Equals(failure);

	/// <inheritdoc/>
	public override int GetHashCode() =>
		Value.GetHashCode();

	/// <inheritdoc/>
	public static bool operator ==(Failure left, Failure right) =>
		left.Equals(right);

	/// <inheritdoc/>
	public static bool operator !=(Failure left, Failure right) =>
		!left.Equals(right);
}
