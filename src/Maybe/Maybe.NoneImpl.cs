// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Maybe<T>
{
	/// <summary>
	/// Generic None class (see <seealso cref="None"/>) that can be returned as a <see cref="Maybe{T}"/> object.
	/// </summary>
	internal sealed record class NoneImpl : Maybe<T>, ILeft<None, T>
	{
		/// <summary>
		/// The actual <see cref="None"/> value.
		/// </summary>
		public None Value { get; init; }

		/// <summary>
		/// Internal creation only.
		/// </summary>
		internal NoneImpl() =>
			Value = M.None;
	}
}
