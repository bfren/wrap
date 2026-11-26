// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public readonly partial struct Either<TLeft, TRight>
{
	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Left{T}"/> - see <see cref="E.WrapLeft{TLeft, TRight}(TLeft)"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="Left{T}"/> and <see cref="Right{T}"/> convert to <see cref="Either{TLeft, TRight}"/> through the union
	/// constructors, so they need no operator here.
	/// </para>
	/// <para>
	/// When a value converts to both <typeparamref name="TLeft"/> and <typeparamref name="TRight"/>, the compiler chooses
	/// between this conversion and the one from <typeparamref name="TRight"/>. If the two types are the same, or the value
	/// converts equally well to both (for example a class that implements both of two interfaces), the conversion is
	/// ambiguous and does not compile (CS0457). If one type is a better match, the compiler picks it without warning: with
	/// <c>Either&lt;object, string&gt;</c>, a <see cref="string"/> becomes a <see cref="Right{T}"/>. In these cases, say
	/// which side you mean with <c>new Left&lt;T&gt;(value)</c>, <c>new Right&lt;T&gt;(value)</c>,
	/// <see cref="E.WrapLeft{TLeft, TRight}(TLeft)"/> or <see cref="E.WrapRight{TLeft, TRight}(TRight)"/>.
	/// </para>
	/// </remarks>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A <see cref="Left{T}"/> holding <paramref name="value"/>.</returns>
	public static implicit operator Either<TLeft, TRight>(TLeft value) =>
		new Left<TLeft>(value);

	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Right{T}"/> - see <see cref="E.WrapRight{TLeft, TRight}(TRight)"/>.
	/// </summary>
	/// <remarks>
	/// When a value converts to both <typeparamref name="TLeft"/> and <typeparamref name="TRight"/>, the conversion is
	/// ambiguous (CS0457) or the compiler picks the better match without warning - see the remarks on the conversion from
	/// <typeparamref name="TLeft"/>. Say which side you mean with <c>new Right&lt;T&gt;(value)</c> or
	/// <see cref="E.WrapRight{TLeft, TRight}(TRight)"/>.
	/// </remarks>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A <see cref="Right{T}"/> holding <paramref name="value"/>.</returns>
	public static implicit operator Either<TLeft, TRight>(TRight value) =>
		new Right<TRight>(value);
}
