// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Maybe<T>
{
	/// <inheritdoc/>
	public virtual bool Equals(Maybe<T>? other) =>
		this switch
		{
			Some<T> x when other is Some<T> y =>
				Equals(x.Value, y.Value),

			None when other is None =>
				true,

			_ =>
				false
		};

	/// <inheritdoc/>
	public override int GetHashCode() =>
		M.Match(this,
			none: M.None.GetHashCode,
			some: x => x.GetHashCode()
		);

	/// <summary>
	/// Compare a Maybe type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Some{T}"/> the <see cref="Some{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Some{T}"/> and its value equals <paramref name="r"/>.</returns>
	public static bool operator ==(Maybe<T> l, T r) =>
		M.Match(l,
			none: () => false,
			some: x => Equals(x, r)
		);

	/// <summary>
	/// Compare a Maybe type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Some{T}"/> the <see cref="Some{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Maybe.</param>
	/// <param name="r">Value.</param>
	/// <returns>True if <paramref name="l"/> is <see cref="Some{T}"/> and its value does not equal <paramref name="r"/>.</returns>
	public static bool operator !=(Maybe<T> l, T r) =>
		M.Match(l,
			none: () => true,
			some: x => !Equals(x, r)
		);

	/// <summary>
	/// Compare a Maybe type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Some{T}"/> the <see cref="Some{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Maybe.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Some{T}"/> and its value equals <paramref name="l"/>.</returns>
	public static bool operator ==(T l, Maybe<T> r) =>
		M.Match(r,
			none: () => false,
			some: x => Equals(x, l)
		);

	/// <summary>
	/// Compare a Maybe type with a value type.
	/// <para>If <paramref name="l"/> is a <see cref="Some{T}"/> the <see cref="Some{T}.Value"/> will be compared to <paramref name="r"/>.</para>
	/// </summary>
	/// <param name="l">Value.</param>
	/// <param name="r">Maybe.</param>
	/// <returns>True if <paramref name="r"/> is <see cref="Some{T}"/> and its value does not equal <paramref name="l"/>.</returns>
	public static bool operator !=(T l, Maybe<T> r) =>
		M.Match(r,
			none: () => true,
			some: x => !Equals(x, l)
		);
}
