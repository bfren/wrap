// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> holds a <typeparamref name="TRight"/> value and get it.
	/// </summary>
	/// <remarks>
	/// If <paramref name="this"/> holds a <typeparamref name="TLeft"/> value, an <see cref="UnsafeUnwrapException"/>
	/// is thrown. Its message includes the Left value, and its inner exception is the Left value's exception
	/// when the Left value implements <see cref="IWithException"/> (for example a <see cref="Failure"/>).
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <returns>The Right value held by <see cref="Unsafe{TEither, TLeft, TRight}.Value"/>.</returns>
	/// <exception cref="UnsafeUnwrapException">Thrown when <see cref="Unsafe{TEither, TLeft, TRight}.Value"/> holds a Left value.</exception>
	public static TRight Unwrap<TEither, TLeft, TRight>(this Unsafe<TEither, TLeft, TRight> @this)
		where TEither : IEither<TLeft, TRight> =>
		@this.Value.TryGetRight(out var right) ? right : ThrowUnwrapException<TEither, TLeft, TRight>(@this.Value);

	/// <summary>
	/// Throw an <see cref="UnsafeUnwrapException"/> for the Left value held by <paramref name="either"/>.
	/// </summary>
	/// <remarks>
	/// Kept out of <see cref="Unwrap{TEither, TLeft, TRight}(Unsafe{TEither, TLeft, TRight})"/> so the JIT can inline the success path.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="either">Either object holding a Left value.</param>
	/// <returns>Never returns.</returns>
	/// <exception cref="UnsafeUnwrapException">Always thrown.</exception>
	private static TRight ThrowUnwrapException<TEither, TLeft, TRight>(TEither either)
		where TEither : IEither<TLeft, TRight>
	{
		_ = either.TryGetLeft(out var left);
		throw new UnsafeUnwrapException(left, (left as IWithException)?.Exception);
	}

	/// <inheritdoc cref="Unwrap{TEither, TLeft, TRight}(Unsafe{TEither, TLeft, TRight})"/>
	public static Task<TRight> UnwrapAsync<TEither, TLeft, TRight>(this Task<Unsafe<TEither, TLeft, TRight>> @this)
		where TEither : IEither<TLeft, TRight> =>
		@this.Then(x => x.Unwrap());
}
