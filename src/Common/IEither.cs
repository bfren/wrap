// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap;

/// <inheritdoc cref="IEither{TEither, TLeft, TRight}"/>
public interface IEither<TLeft, TRight>
{
	/// <remarks>
	/// <para>
	/// Provides access to the value wrapped by a this object if it is a <see cref="IRight{TLeft, TRight}"/>.
	/// </para>
	/// <para>
	/// You need to provide a default value via <paramref name="getValue"/> in case the object is
	/// is a <see cref="ILeft{TLeft, TRight}"/>.
	/// </para>
	/// </remarks>
	/// <param name="getValue">Used to handle error / invalid state and return correct / valid value if
	/// the object is <typeparamref name="TLeft"/>.</param>
	/// <returns>Value of object or provided by <paramref name="getValue"/>.</returns>
	TRight Unwrap(Func<TLeft, TRight> getValue);

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

/// <summary>
/// Either Monad.
/// </summary>
/// <typeparam name="TEither">Either implementation type.</typeparam>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
public interface IEither<TEither, TLeft, TRight> : IEither<TLeft, TRight>
	where TEither : IEither<TLeft, TRight>
{
	/// <summary>
	/// Shorthand for returning the current object as a task.
	/// </summary>
	/// <returns>Task result.</returns>
	Task<TEither> AsTask();
}
