// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

public static partial class FailExtensions
{
	/// <summary>
	/// Wrap a <see cref="Fail"/> object as a Task for simple async returns.
	/// </summary>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="this">Fail object</param>
	/// <returns>Result task.</returns>
	public static Task<Result<T>> AsTask<T>(this Fail @this) =>
		Result<T>.Failure.Create(@this.Value).AsTask();
}
