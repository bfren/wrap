// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq;

namespace Wrap;

public readonly partial record struct Failure
{
	/// <summary>
	/// Compare this failure with <paramref name="other"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="Args"/> are compared item by item, so separate arrays with equal values are equal.
	/// <see cref="Exception"/> objects are compared by reference.
	/// </remarks>
	/// <param name="other">Failure to compare.</param>
	/// <returns>
	/// <see langword="true"/> if <see cref="Message"/>, <see cref="Args"/>, <see cref="Context"/>,
	/// <see cref="Exception"/> and <see cref="Level"/> are all equal.
	/// </returns>
	public bool Equals(Failure other) =>
		Message == other.Message
		&& Level == other.Level
		&& Context == other.Context
		&& Equals(Exception, other.Exception)
		&& (Args ?? []).SequenceEqual(other.Args ?? []);

	/// <summary>
	/// Get a hash code that is the same for equal failures - see <see cref="Equals(Failure)"/>.
	/// </summary>
	/// <returns>Hash code.</returns>
	public override int GetHashCode() =>
		(Args ?? []).Aggregate(
			HashCode.Combine(Message, Level, Context, Exception),
			(hash, arg) => HashCode.Combine(hash, arg)
		);
}
