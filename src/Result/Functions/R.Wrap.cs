// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class R
{
	/// <summary>
	/// Return a 'true' <see cref="Ok{T}"/> result.
	/// </summary>
	/// <returns><see cref="Ok{T}"/> result containing the value <c>true</c>.</returns>
	public static Result<bool> Ok() =>
		Wrap(true);

	/// <summary>
	/// One of the most important functions in the library: takes a value and returns
	/// either <see cref="Ok{T}"/> or <see cref="Failure"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="value"/> is not null, you will get an <see cref="Ok{T}"/> object where
	/// <see cref="Ok{T}.Value"/> is <paramref name="value"/>.
	/// </para>
	/// <para>
	/// If <paramref name="value"/> is null, you will get a <see cref="Failure"/> object.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="value">Value to wrap.</param>
	/// <returns>
	/// <see cref="Ok{T}"/> if <paramref name="value"/> is not null - otherwise <see cref="Failure"/>.
	/// </returns>
	public static Result<T> Wrap<T>(T value) =>
		value switch
		{
			T =>
				new Ok<T>(value),

			_ =>
				Fail(
					"Null value of type '{Type}' - try using Maybe<T> to wrap null values safely.",
					typeof(T).Name
				),
		};
}
