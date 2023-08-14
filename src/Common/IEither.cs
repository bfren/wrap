// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;

namespace Monadic;

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
	///     // 'right' is TRight
	///     // if 'either' is TLeft, the loop is empty
	/// }
	/// </code>
	/// </remarks>
	/// <returns>Enumerator containing one value if this is <typeparamref name="TRight"/>.</returns>
	IEnumerator<TRight> GetEnumerator();
}
