// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Wrap;

/// <summary>
/// Wrapper that gives access to unsafe functions, which may throw or return <see langword="null"/> values.
/// </summary>
/// <typeparam name="TEither">Either implementation type.</typeparam>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public readonly record struct Unsafe<TEither, TLeft, TRight> : IMonad<TEither>
	where TEither : IEither<TLeft, TRight>
{
	/// <summary>
	/// Gets the wrapped <typeparamref name="TEither"/> object.
	/// </summary>
	public TEither Value { get; init; }

	/// <summary>
	/// Wrap an either object; use the <c>Unsafe()</c> extension methods to create one.
	/// </summary>
	/// <param name="value"><typeparamref name="TEither"/> object to wrap.</param>
	internal Unsafe(TEither value) =>
		Value = value;

	/// <summary>
	/// Get an enumerator over the right value, so it can be read with <see langword="foreach"/>.
	/// </summary>
	/// <remarks>
	/// For example:
	/// <code>
	/// foreach (var right in either.Unsafe()) {
	///     // if 'either' holds a left value, the loop body does not run
	///     // otherwise 'right' is the right value
	/// }
	/// </code>
	/// </remarks>
	/// <returns>An enumerator with one item if <see cref="Value"/> holds a right value, otherwise an empty enumerator.</returns>
	public IEnumerator<TRight> GetEnumerator()
	{
		// keep this a yield iterator: the JIT can then stack-allocate it in an inlined foreach (0 B), which a collection prevents
		if (Value.TryGetRight(out var value))
		{
			yield return value;
		}
	}
}
