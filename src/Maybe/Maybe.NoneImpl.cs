// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// maybe.
/// </summary>
public abstract partial record class Maybe<T>
{
	/// <summary>
	/// Cache NoneImpl value.
	/// </summary>
	internal static readonly NoneImpl None =
		new();

	/// <summary>
	/// Cache None task.
	/// </summary>
	internal static readonly Task<Maybe<T>> NoneAsTask =
		None.AsTask();

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
