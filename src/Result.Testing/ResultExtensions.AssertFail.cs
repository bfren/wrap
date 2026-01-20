// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap.Testing;

public static partial class ResultExtensions
{
	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Result{T}.Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <returns>The failure value of <paramref name="this"/>.</returns>
	public static FailValue AssertFail<T>(this Result<T> @this) =>
		Assert.IsType<Result<T>.Failure>(@this).Value;

	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Result{T}.Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">Optional arguments to fill in failure message values.</param>
	/// <returns>The failure value of <paramref name="this"/>.</returns>
	public static FailValue AssertFail<T>(this Result<T> @this, string message, params object?[] args)
	{
		// Assert failure and get value
		var value = AssertFail(@this);

		// Assert correct message
		Assert.Equal(message, value.Message);

		// Assert args if provided
		if (args?.Length > 0)
		{
			Assert.Equal(args, value.Args);
		}

		// Return FailValue
		return value;
	}

	/// <summary>
	/// Assert that <paramref name="this"/> is <see cref="Result{T}.Failure"/>.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Result object.</param>
	/// <param name="ex">Expected exception message.</param>
	/// <param name="message">Expected failure message.</param>
	/// <param name="args">Optional arguments to fill in failure message values.</param>
	/// <returns>The failure value of <paramref name="this"/>.</returns>
	public static FailValue AssertFail<T>(this Result<T> @this, Exception ex, string message, params object?[] args)
	{
		// Assert failure and get value
		var value = AssertFail(@this, message, args);

		// Assert correct Exception
		Assert.Equal(ex, value.Exception);

		// Return FailValue
		return value;
	}
}
