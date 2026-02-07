// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap.Extensions;

public static partial class ResultExtensions
{
	/// <summary>
	/// Returns <see langword="false"/> if <paramref name="this"/> is <see cref="Failure"/>,
	/// or the value of <paramref name="this"/>.
	/// </summary>
	/// <param name="this">Result object.</param>
	/// <returns>Whether or not the value of <paramref name="this"/> is <see langword="false"/>.</returns>
	public static bool IsFalse(this Result<bool> @this) =>
		R.Match(@this,
			fFail: _ => false,
			fOk: x => !x
		);

	/// <inheritdoc cref="IsTrue(Result{bool})"/>
	public static Task<bool> IsFalseAsync(this Task<Result<bool>> @this) =>
		R.MatchAsync(@this,
			fFail: _ => false,
			fOk: x => !x
		);
}
