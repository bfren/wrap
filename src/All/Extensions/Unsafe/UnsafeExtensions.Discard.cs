// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Discard the Left (error / invalid) value of an <see cref="IEither{TLeft, TRight}"/>
	/// and return a <see cref="Maybe{T}"/> of the Right (correct / valid) value.
	/// </summary>
	/// <remarks>
	/// WARNING: this will discard the left value that gives details of the error / invalid operation.
	/// You should only use this method if you have logged it, or *really* don't care.
	/// </remarks>
	/// <typeparam name="TEither">Either implementation type.</typeparam>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <returns><see cref="Some{T}"/> holding the Right value, or <see cref="None"/> if <paramref name="this"/> holds a Left value or the Right value is <see langword="null"/>.</returns>
	public static Maybe<TRight> Discard<TEither, TLeft, TRight>(this Unsafe<TEither, TLeft, TRight> @this)
		where TEither : IEither<TLeft, TRight> =>
		@this.Value.TryGetRight(out var right) ? M.Wrap(right) : default;

	/// <inheritdoc cref="Discard{TEither, TLeft, TRight}(Unsafe{TEither, TLeft, TRight})"/>
	public static Task<Maybe<TRight>> DiscardAsync<TEither, TLeft, TRight>(this Task<Unsafe<TEither, TLeft, TRight>> @this)
		where TEither : IEither<TLeft, TRight> =>
		@this.Then(x => x.Discard());
}
