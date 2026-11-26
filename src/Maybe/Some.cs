// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text;

namespace Wrap;

/// <summary>
/// The 'Some' case of <see cref="Maybe{T}"/>, which holds a value (see <seealso cref="None"/>).
/// </summary>
/// <remarks>
/// A <see cref="Some{T}"/> holding <see langword="null"/> becomes <see cref="None"/> when it is
/// converted to <see cref="Maybe{T}"/>, so <see cref="Value"/> is never <see langword="null"/> when you
/// match it from a <see cref="Maybe{T}"/>.
/// </remarks>
/// <typeparam name="T">Some value type.</typeparam>
/// <param name="Value">Some value.</param>
public readonly record struct Some<T>(T Value) : IMonad<T>
{
	// print the value using F.DefaultCulture, as everywhere else, not the current culture
	private bool PrintMembers(StringBuilder builder)
	{
		_ = builder.Append("Value = ").Append(Convert.ToString(Value, F.DefaultCulture));
		return true;
	}
}
