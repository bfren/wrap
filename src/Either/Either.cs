// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Either monad: a union that holds either a <see cref="Left{T}"/> (error / invalid) value or a
/// <see cref="Right{T}"/> (correct / valid) value.
/// </summary>
/// <remarks>
/// <para>
/// Use pattern matching to read the value - the compiler checks that both cases are handled:
/// </para>
/// <code>
/// var text = either switch
/// {
///     Left&lt;string&gt; l => $"Error: {l.Value}",
///     Right&lt;int&gt; r => $"Value: {r.Value}"
/// };
/// </code>
/// <para>
/// This is a value type: <c>default(Either&lt;TLeft, TRight&gt;)</c> is a <see cref="Left{T}"/> holding
/// <c>default(TLeft)</c>, and an <see cref="Either{TLeft, TRight}"/> can never be <see langword="null"/>.
/// </para>
/// <para>
/// Unlike <c>Some&lt;T&gt;</c> or <c>Ok&lt;T&gt;</c>, either case may hold <see langword="null"/>.
/// </para>
/// </remarks>
/// <typeparam name="TLeft">Left (error / invalid) value type.</typeparam>
/// <typeparam name="TRight">Right (correct / valid) value type.</typeparam>
[Union]
public readonly partial struct Either<TLeft, TRight> : IEither<TLeft, TRight>, IEquatable<Either<TLeft, TRight>>
{
	/// <summary>
	/// The left value - only meaningful when <see cref="isRight"/> is <see langword="false"/>.
	/// </summary>
	private readonly TLeft? left;

	/// <summary>
	/// The right value - only meaningful when <see cref="isRight"/> is <see langword="true"/>.
	/// </summary>
	private readonly TRight? right;

	/// <summary>
	/// Whether this object is a <see cref="Right{T}"/> (<see langword="true"/>) or a <see cref="Left{T}"/> (<see langword="false"/>).
	/// </summary>
	private readonly bool isRight;

	/// <summary>
	/// Gets <see langword="true"/> always: the union always holds a case, because <c>default</c> is a <see cref="Left{T}"/>.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>is</c> patterns and <c>switch</c>, so it knows an <see cref="Either{TLeft, TRight}"/>
	/// is never <see langword="null"/> and no <see langword="null"/> case is needed.
	/// </remarks>
	public bool HasValue =>
		true;

	/// <summary>
	/// Gets the current case, boxed as <see cref="Left{T}"/> or <see cref="Right{T}"/>.
	/// </summary>
	/// <remarks>
	/// Required by the union pattern and used by reflection and serialisers. The compiler calls it for
	/// patterns that neither <see cref="TryGetValue(out Left{TLeft})"/> nor <see cref="TryGetValue(out Right{TRight})"/> covers.
	/// It allocates on every call - use pattern matching instead.
	/// </remarks>
	public object? Value =>
		isRight switch
		{
			true =>
				new Right<TRight>(right!),

			false =>
				new Left<TLeft>(left!)
		};

	/// <summary>
	/// Create an <see cref="Either{TLeft, TRight}"/> from a <see cref="Left{T}"/> value.
	/// </summary>
	/// <remarks>
	/// The compiler also uses this constructor for the implicit conversion from <see cref="Left{T}"/>.
	/// </remarks>
	/// <param name="left">Left value.</param>
	public Either(Left<TLeft> left) =>
		(this.left, isRight) = (left.Value, false);

	/// <summary>
	/// Create an <see cref="Either{TLeft, TRight}"/> from a <see cref="Right{T}"/> value.
	/// </summary>
	/// <remarks>
	/// The compiler also uses this constructor for the implicit conversion from <see cref="Right{T}"/>.
	/// </remarks>
	/// <param name="right">Right value.</param>
	public Either(Right<TRight> right) =>
		(this.right, isRight) = (right.Value, true);

	/// <summary>
	/// Wrap this object in a completed task.
	/// </summary>
	/// <returns>A completed task holding this object.</returns>
	public Task<Either<TLeft, TRight>> AsTask() =>
		Task.FromResult(this);
}
