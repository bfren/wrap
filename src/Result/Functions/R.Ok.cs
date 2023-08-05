// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Monadic;

public static partial class R
{
	public static Result<T> Ok<T>(T value) =>
		new Ok<T>(value);

	public static Task<Result<T>> OkAsync<T>(T value) =>
		Ok(value).AsTask();
}
