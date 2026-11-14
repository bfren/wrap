// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Check whether <paramref name="this"/> is <see cref="Ok{T}"/> holding <see langword="false"/>.
	/// </summary>
	/// <param name="this">Result object.</param>
	/// <returns><see langword="true"/> if <paramref name="this"/> holds <see langword="false"/>; <see langword="false"/> if it holds <see langword="true"/> or is a <see cref="Failure"/>.</returns>
	public static bool IsFalse(this Result<bool> @this) =>
		R.Match(@this,
			fFail: _ => false,
			fOk: x => !x
		);

	/// <inheritdoc cref="IsFalse(Result{bool})"/>
	public static Task<bool> IsFalseAsync(this Task<Result<bool>> @this) =>
		R.MatchAsync(@this,
			fFail: _ => false,
			fOk: x => !x
		);
}
