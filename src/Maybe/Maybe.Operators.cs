// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public readonly partial struct Maybe<T>
{
	/// <summary>
	/// Wrap <paramref name="value"/> as a <see cref="Maybe{T}"/> - see <see cref="M.Wrap{T}(T)"/>.
	/// </summary>
	/// <remarks>
	/// <see cref="Some{T}"/> and <see cref="None"/> convert to <see cref="Maybe{T}"/> through the union constructors,
	/// so they need no operator here.
	/// </remarks>
	/// <param name="value">Value to wrap.</param>
	/// <returns><see cref="Some{T}"/> if <paramref name="value"/> is not <see langword="null"/>, otherwise <see cref="None"/>.</returns>
	public static implicit operator Maybe<T>(T value) =>
		M.Wrap(value);

	/// <summary>
	/// Unwrap <paramref name="obj"/> and wrap its value as a <see cref="Maybe{T}"/> - see <see cref="M.Wrap{T}(T)"/>.
	/// </summary>
	/// <param name="obj">Monad to convert.</param>
	/// <returns>
	/// <see cref="Some{T}"/> holding the value of <paramref name="obj"/>, or <see cref="None"/> if <paramref name="obj"/> is <see langword="null"/>.
	/// </returns>
	public static implicit operator Maybe<T>(Monad<T> obj) =>
		obj switch
		{
			Monad<T> =>
				M.Wrap(obj.Value),

			_ =>
				M.None
		};
}
