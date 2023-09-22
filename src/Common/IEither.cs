// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Monads;

/// <summary>
/// Either Monad.
/// </summary>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface IEither<out TLeft, out TRight>
{
	/// <summary>
	/// Use enumerator pattern to get <typeparamref name="TRight"/> value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// foreach (var right in either) {
	///     // if 'either' is <typeparamref name="TLeft"/>, the loop is empty
	///     // otherwise 'right' is <typeparamref name="TRight"/><br/>
	/// }
	/// </code>
	/// </remarks>
	/// <returns>Enumerator containing one value if this is <typeparamref name="TRight"/>.</returns>
	IEnumerator<TRight> GetEnumerator();
}
