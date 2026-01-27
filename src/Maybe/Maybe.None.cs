// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Maybe<T>
{
	/// <summary>
	/// Generic None class (see <seealso cref="Wrap.None"/>) that can be returned as a <see cref="Maybe{T}"/> object.
	/// </summary>
	internal sealed record class None : Maybe<T>, ILeft<Wrap.None, T>
	{
		/// <summary>
		/// The actual <see cref="Wrap.None"/> value.
		/// </summary>
		public Wrap.None Value { get; init; }

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
