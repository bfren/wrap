// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Pass an existing failure on to a new <see cref="Result{T}"/> without logging it again -
	/// it was logged when it was first created.
	/// </summary>
	/// <typeparam name="T">Result value type.</typeparam>
	/// <param name="value">Failure value.</param>
	/// <returns>Failure result.</returns>
	internal static Result<T> Pass<T>(FailureValue value) =>
		new Result<T>.FailureImpl(null, null, value);
}
