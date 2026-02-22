// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Threading.Tasks;

namespace Wrap;

/// <inheritdoc cref="IEither{TLeft, TRight}"/>
public abstract record class Either<TLeft, TRight> : IEither<Either<TLeft, TRight>, TLeft, TRight>
{
	/// <inheritdoc/>
	public Task<Either<TLeft, TRight>> AsTask() =>
		Task.FromResult(this);

	/// <inheritdoc/>
	public TRight Unwrap(Func<TLeft, TRight> getValue) =>
		E.Match<Either<TLeft, TRight>, TLeft, TRight, TRight>(this,
			fLeft: l => getValue(l),
			fRight: x => x
		);

	/// <summary>
	/// Implicitly convert a <typeparamref name="TLeft"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	public static implicit operator Either<TLeft, TRight>(TLeft value) =>
		E.WrapLeft<TLeft, TRight>(value);

	/// <summary>
	/// Implicitly convert a <see cref="Monad{T}"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Either<TLeft, TRight>(Monad<TLeft> obj) =>
		E.WrapLeft<TLeft, TRight>(obj.Value);

	/// <summary>
	/// Implicitly convert a <typeparamref name="TRight"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	public static implicit operator Either<TLeft, TRight>(TRight value) =>
		E.WrapRight<TLeft, TRight>(value);

	/// <summary>
	/// Implicitly convert a <see cref="Monad{T}"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Either<TLeft, TRight>(Monad<TRight> obj) =>
		E.WrapRight<TLeft, TRight>(obj.Value);
}
