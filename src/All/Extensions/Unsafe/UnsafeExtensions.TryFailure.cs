// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Extensions;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// If <paramref name="this"/> contains a <see cref="Failure"/>, set <paramref name="value"/>
	/// to be <see cref="Failure.Value"/>.
	/// </summary>
	/// <remarks>
	/// Warning: if <paramref name="this"/> contains an <see cref="Ok{T}"/> <paramref name="value"/>
	/// will be the default value of <typeparamref name="T"/> or null.
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	/// <param name="value">Value (null if <paramref name="this"/> contains <see cref="Ok{T}"/>).</param>
	/// <returns>True if <paramref name="this"/> contains a <see cref="Failure"/>.</returns>
	public static bool TryFailure<T>(this Unsafe<Result<T>, FailureValue, T> @this, out FailureValue value)
	{
		if (@this.Value is Result<T>.FailureImpl failure)
		{
			value = failure.Value;
			return true;
		}

		value = default!;
		return false;
	}
}
