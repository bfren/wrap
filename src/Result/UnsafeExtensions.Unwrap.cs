// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> is a <see cref="Ok{T}"/> and get the value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// However (!) if <paramref name="this"/> is a <see cref="Fail"/>, you will get a null value.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	public static T Unwrap<T>(this Unsafe<T> @this) =>
		@this.Result.Unwrap(_ => default!);

	/// <inheritdoc cref="Unwrap{T}(Unsafe{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Unsafe<T>> @this) =>
		Unwrap(await @this);
}
