// Wrap: .NET monads.
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
			left: l => getValue(l),
			right: x => x
		);

	/// <summary>
	/// Implicitly convert a <see cref="Union{T}"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Either<TLeft, TRight>(Union<TLeft> obj) =>
		E.WrapLeft<TLeft, TRight>(obj.Value);

	/// <summary>
	/// Implicitly convert a <see cref="Union{T}"/> into a <see cref="Either{TLeft, TRight}"/> object.
	/// </summary>
	/// <param name="obj">Wrapped object.</param>
	public static implicit operator Either<TLeft, TRight>(Union<TRight> obj) =>
		E.WrapRight<TLeft, TRight>(obj.Value);
}
