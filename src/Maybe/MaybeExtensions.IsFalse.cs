// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Monads;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Returns <see langword="false"/> if <paramref name="this"/> is <see cref="None"/>,
	/// or the value of <paramref name="this"/>.
	/// </summary>
	/// <param name="this">Maybe object.</param>
	/// <returns>Whether or not the value of <paramref name="this"/> is <see langword="false"/>.</returns>
	public static bool IsFalse(this Maybe<bool> @this) =>
		M.Switch(@this,
			none: false,
			some: x => x == false
		);

	/// <inheritdoc cref="IsTrue(Maybe{bool})"/>
	public static Task<bool> IsFalseAsync(this Task<Maybe<bool>> @this) =>
		M.SwitchAsync(@this,
			none: false,
			some: x => x == false
		);
}
