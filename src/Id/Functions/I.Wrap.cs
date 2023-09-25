// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class I
{
	/// <summary>
	/// Wrap a value as a strongly-typed ID.
	/// </summary>
	/// <typeparam name="TId">ID type.</typeparam>
	/// <typeparam name="TValue">ID value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Value wrapped as a strongly-typed ID.</returns>
	public static TId Wrap<TId, TValue>(TValue value)
		where TId : Id<TId, TValue>, new()
		where TValue : notnull =>
		new() { Value = value };
}
