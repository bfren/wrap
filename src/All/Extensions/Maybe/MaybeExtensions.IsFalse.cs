// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class MaybeExtensions
{
	/// <summary>
	/// Check whether <paramref name="this"/> is <see cref="Some{T}"/> holding <see langword="false"/>.
	/// </summary>
	/// <param name="this">Maybe object.</param>
	/// <returns><see langword="true"/> if <paramref name="this"/> holds <see langword="false"/>; <see langword="false"/> if it holds <see langword="true"/> or is <see cref="None"/>.</returns>
	public static bool IsFalse(this Maybe<bool> @this) =>
		M.Match(@this,
			fNone: () => false,
			fSome: x => !x
		);

	/// <inheritdoc cref="IsFalse(Maybe{bool})"/>
	public static Task<bool> IsFalseAsync(this Task<Maybe<bool>> @this) =>
		M.MatchAsync(@this,
			fNone: () => false,
			fSome: x => !x
		);
}
