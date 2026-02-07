// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Discard the Left (error / invalid) value of a <see cref="IEither{TEither, TLeft, TRight}"/>
	/// and return <see cref="M.None"/> or <see cref="Some{T}"/> with the Right (correct / valid) value.
	/// </summary>
	/// <remarks>
	/// WARNING: this will discard the left value that gives details of the error / invalid operation.
	/// You should only use this method if you have logged it, or *really* don't care.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <returns>Maybe object.</returns>
	public static Maybe<TRight> Discard<TEither, TLeft, TRight>(this Unsafe<TEither, TLeft, TRight> @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		E.Match<TEither, TLeft, TRight, Maybe<TRight>>(
			@this.Value,
			fLeft: _ => M.None,
			fRight: M.Wrap
		);

	/// <inheritdoc cref="Discard{TEither, TLeft, TRight}(Unsafe{TEither, TLeft, TRight})"/>
	public static async Task<Maybe<TRight>> DiscardAsync<TEither, TLeft, TRight>(this Task<Unsafe<TEither, TLeft, TRight>> @this)
		where TEither : IEither<TEither, TLeft, TRight> =>
		await E.MatchAsync<TEither, TLeft, TRight, Maybe<TRight>>(
			(await @this).Value,
			fLeft: _ => M.None,
			fRight: async x => x
		);
}
