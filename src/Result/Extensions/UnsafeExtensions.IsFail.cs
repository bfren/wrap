// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// If <paramref name="this"/> contains a <see cref="Fail"/>, set <paramref name="value"/>
	/// to be <see cref="Fail.Value"/>.
	/// </summary>
	/// <remarks>
	/// Warning: if <paramref name="this"/> contains an <see cref="Ok{T}"/> <paramref name="value"/>
	/// will be the default value of <typeparamref name="T"/> or null.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <param name="value">Value (null if <paramref name="this"/> contains <see cref="Ok{T}"/>).</param>
	/// <returns>True if <paramref name="this"/> contains a <see cref="Fail"/>.</returns>
	public static bool IsFail<T>(this Unsafe<T> @this, out FailValue value)
	{
		if (@this.Result is Result<T>.Failure failure)
		{
			value = failure.Value;
			return true;
		}

		value = default!;
		return false;
	}
}
