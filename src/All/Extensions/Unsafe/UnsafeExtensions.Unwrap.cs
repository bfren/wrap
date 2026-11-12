// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;
using Wrap.Exceptions;

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> is a <typeparamref name="TRight"/> and get the value.
	/// </summary>
	/// <remarks>
	/// However (!) if <paramref name="this"/> is a <typeparamref name="TLeft"/>, you will get
	/// the default value of <typeparamref name="TRight"/> or null.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <returns>Value of <see cref="Unsafe{TEither, TLeft, TRight}.Value"/>.</returns>
	public static TRight Unwrap<TEither, TLeft, TRight>(this Unsafe<TEither, TLeft, TRight> @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		@this.Value.Match(
			fLeft: x => throw new UnsafeUnwrapException(x, (x as IWithException)?.Exception),
			fRight: x => x
		);

	/// <inheritdoc cref="Unwrap{TEither, TLeft, TRight}(Unsafe{TEither, TLeft, TRight})"/>
	public static Task<TRight> UnwrapAsync<TEither, TLeft, TRight>(this Task<Unsafe<TEither, TLeft, TRight>> @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		@this.Then(x => x.Unwrap());
}
