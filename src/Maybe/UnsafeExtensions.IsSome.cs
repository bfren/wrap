// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// If <paramref name="this"/> contains a <see cref="Some{T}"/>, set <paramref name="value"/>
	/// to be <see cref="Some{T}.Value"/>.
	/// </summary>
	/// <remarks>
	/// Warning: if <paramref name="this"/> contains a <see cref="None"/> <paramref name="value"/>
	/// will be the default value of <typeparamref name="T"/> or null.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <param name="value">Value (default of <typeparamref name="T"/> or null if <paramref name="this"/> contains <see cref="None"/>).</param>
	/// <returns>True if <paramref name="this"/> contains a <see cref="Some{T}"/>.</returns>
	public static bool IsSome<T>(this Unsafe<T> @this, out T value)
	{
		if (@this.Maybe is Some<T> some)
		{
			value = some.Value;
			return true;
		}

		value = default!;
		return false;
	}
}
