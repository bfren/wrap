// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// If <paramref name="this"/> contains a <see cref="Ok{T}"/>, set <paramref name="value"/>
	/// to be <see cref="Ok{T}.Value"/>.
	/// </summary>
	/// <remarks>
	/// Warning: if <paramref name="this"/> contains a <see cref="Fail"/> <paramref name="value"/>
	/// will be the default value of <typeparamref name="T"/> or null.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <param name="value">Value (null if <paramref name="this"/> contains <see cref="Fail"/>).</param>
	/// <returns>True if <paramref name="this"/> contains a <see cref="Ok{T}"/>.</returns>
	public static bool IsOk<T>(this Unsafe2<Result<T>, FailValue, T> @this, out T value)
	{
		if (@this.Value is Ok<T> ok)
		{
			value = ok.Value;
			return true;
		}

		value = default!;
		return false;
	}
}
