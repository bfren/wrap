// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

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
	public static bool TrySome<T>(this Unsafe<Maybe<T>, None, T> @this, out T value)
	{
		if (@this.Value is Some<T> some)
		{
			value = some.Value;
			return true;
		}

		value = default!;
		return false;
	}
}
