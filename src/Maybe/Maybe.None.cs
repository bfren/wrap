// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public abstract partial record class Maybe<T>
{
	/// <summary>
	/// Generic None class (see <seealso cref="Monads.None"/>) that can be returned as a <see cref="Maybe{T}"/> object.
	/// </summary>
	internal sealed record class None : Maybe<T>, ILeft<Monads.None, T>
	{
		/// <summary>
		/// The actual <see cref="Monads.None"/> value.
		/// </summary>
		public Monads.None Value { get; private init; }

		/// <summary>
		/// Private creation only - see <see cref="Create"/>.
		/// </summary>
		private None() =>
			Value = new();

		/// <summary>
		/// Create a <see cref="None"/> object.
		/// </summary>
		/// <returns><see cref="None"/> object as the more generic base <see cref="Maybe{T}"/>.</returns>
		internal static Maybe<T> Create() =>
			new None();
	}
}
