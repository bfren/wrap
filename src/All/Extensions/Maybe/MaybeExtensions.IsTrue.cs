// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Returns <see langword="false"/> if <paramref name="this"/> is <see cref="None"/>,
	/// or the value of <paramref name="this"/>.
	/// </summary>
	/// <param name="this">Maybe object.</param>
	/// <returns>Whether or not the value of <paramref name="this"/> is <see langword="true"/>.</returns>
	public static bool IsTrue(this Maybe<bool> @this) =>
		M.Match(@this,
			none: false,
			some: x => x
		);

	/// <inheritdoc cref="IsTrue(Maybe{bool})"/>
	public static Task<bool> IsTrueAsync(this Task<Maybe<bool>> @this) =>
		M.MatchAsync(@this,
			none: false,
			some: x => x
		);
}
