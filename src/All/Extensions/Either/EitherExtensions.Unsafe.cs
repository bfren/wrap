// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class EitherExtensions
{
	/// <summary>
	/// Wrap <paramref name="this"/> in an <see cref="Unsafe{TEither, TLeft, TRight}"/> object to reach its Right value directly.
	/// </summary>
	/// <remarks>
	/// Each way of reading the value from <see cref="Unsafe{TEither, TLeft, TRight}"/> drops the Left value:
	/// <c>Unwrap</c> throws, <c>Discard</c> returns <see cref="None"/> and <c>foreach</c> does not run.
	/// </remarks>
	/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
	/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
	/// <param name="this">Either object.</param>
	/// <returns><see cref="Unsafe{TEither, TLeft, TRight}"/> object wrapping <paramref name="this"/>.</returns>
	public static Unsafe<Either<TLeft, TRight>, TLeft, TRight> Unsafe<TLeft, TRight>(this Either<TLeft, TRight> @this) =>
		new(@this);

	/// <inheritdoc cref="Unsafe{TLeft, TRight}(Either{TLeft, TRight})"/>
	public static async Task<Unsafe<Either<TLeft, TRight>, TLeft, TRight>> Unsafe<TLeft, TRight>(this Task<Either<TLeft, TRight>> @this) =>
		new() { Value = await @this };
}
