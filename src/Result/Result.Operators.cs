// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public readonly partial struct Result<T>
{
	/// <summary>
	/// Implicitly wrap <paramref name="value"/> as a <see cref="Result{T}"/> - see <see cref="R.Wrap{T}(T)"/>.
	/// </summary>
	/// <param name="value">Input value.</param>
	/// <returns>
	/// An <see cref="Ok{T}"/> holding <paramref name="value"/>, or a <see cref="Failure"/> if <paramref name="value"/> is <see langword="null"/>.
	/// </returns>
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	/// <summary>
	/// Unwrap <paramref name="obj"/> and wrap its value as a <see cref="Result{T}"/> - see <see cref="R.Wrap{T}(T)"/>.
	/// </summary>
	/// <param name="obj">Monad to convert.</param>
	/// <returns>
	/// An <see cref="Ok{T}"/> holding the value of <paramref name="obj"/>, or a <see cref="Failure"/> if <paramref name="obj"/> is <see langword="null"/>.
	/// </returns>
	public static implicit operator Result<T>(Monad<T> obj) =>
		obj switch
		{
			Monad<T> =>
				R.Wrap(obj.Value),

			_ =>
				R.Fail(Failure.NullValue)
		};
}
