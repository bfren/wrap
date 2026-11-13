// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Maybe monad: a union that holds either a <see cref="Some{T}"/> value or <see cref="None"/>.
/// </summary>
/// <remarks>
/// <para>
/// Use pattern matching to read the value - the compiler checks that both cases are handled:
/// </para>
/// <code>
/// var text = maybe switch
/// {
///     Some&lt;int&gt; x => $"Value: {x.Value}",
///     None => "No value"
/// };
/// </code>
/// <para>
/// This is a value type: <c>default(Maybe&lt;T&gt;)</c> is <see cref="None"/>, and a <see cref="Maybe{T}"/>
/// can never be <see langword="null"/>.
/// </para>
/// </remarks>
/// <typeparam name="T">Some value type.</typeparam>
[Union]
public readonly partial struct Maybe<T> : IEither<Maybe<T>, None, T>, IEquatable<Maybe<T>>
{
	/// <summary>
	/// Always <see langword="true"/>: the union always holds a case, because <c>default</c> is <see cref="None"/>.
	/// </summary>
	/// <remarks>
	/// Required by the union pattern so the compiler knows a <see cref="Maybe{T}"/> is never <see langword="null"/>.
	/// </remarks>
	public bool HasValue =>
		true;

	/// <summary>
	/// The wrapped value - only meaningful when <see cref="isSome"/> is <see langword="true"/>.
	/// </summary>
	private readonly T? value;

	/// <summary>
	/// The current case, boxed as <see cref="Some{T}"/> or <see cref="None"/>.
	/// </summary>
	/// <remarks>
	/// Required by the union pattern and used by reflection and serialisers.
	/// It allocates on every call - use pattern matching instead.
	/// </remarks>
	public object? Value =>
		isSome switch
		{
			true =>
				new Some<T>(value!),

			false =>
				M.None
		};

	/// <summary>
	/// Whether this object is a <see cref="Some{T}"/> (<see langword="true"/>) or <see cref="None"/> (<see langword="false"/>).
	/// </summary>
	private readonly bool isSome;

	/// <summary>
	/// Create a <see cref="Maybe{T}"/> from a <see cref="Some{T}"/> value.
	/// </summary>
	/// <remarks>
	/// If <see cref="Some{T}.Value"/> is <see langword="null"/> the result is <see cref="None"/>,
	/// so a <see cref="Some{T}"/> case never holds <see langword="null"/>.
	/// </remarks>
	/// <param name="some">Some value.</param>
	public Maybe(Some<T> some) =>
		(value, isSome) = (some.Value, some.Value is not null);

	/// <summary>
	/// Create a <see cref="Maybe{T}"/> from a <see cref="None"/> value.
	/// </summary>
	/// <param name="_">None value (discarded).</param>
	public Maybe(None _) { }

	/// <inheritdoc/>
	public Task<Maybe<T>> AsTask() =>
		Task.FromResult(this);

	/// <summary>
	/// Use the enumerator pattern to get the <typeparamref name="T"/> value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// For example:
	/// </para>
	/// <code>
	/// foreach (var x in maybe)
	/// {
	///     // the loop body runs once if 'maybe' is Some&lt;T&gt; (and 'x' is its value),
	///     // and not at all if 'maybe' is None
	/// }
	/// </code>
	/// </remarks>
	/// <returns>An enumerator with one value if this is a <see cref="Some{T}"/>, otherwise an empty enumerator.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		// keep this a yield iterator: the JIT can then stack-allocate it in an inlined foreach (0 B), which a collection prevents
		if (this is Some<T> some)
		{
			yield return some.Value;
		}
	}
}
