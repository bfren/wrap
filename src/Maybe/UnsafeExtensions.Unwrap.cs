// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

public static partial class UnsafeExtensions
{
	/// <summary>
	/// Assume <paramref name="this"/> is a <see cref="Some{T}"/> and get the value.
	/// </summary>
	/// <remarks>
	/// However (!) if <paramref name="this"/> is a <see cref="None"/>, you will get
	/// the default value of <typeparamref name="T"/> or null.
	/// </remarks>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">Unsafe object.</param>
	public static T Unwrap<T>(this Unsafe<T> @this) =>
		@this.Maybe.Unwrap(() => default!);

	/// <inheritdoc cref="Unwrap{T}(Unsafe{T})"/>
	public static async Task<T> UnwrapAsync<T>(this Task<Unsafe<T>> @this) =>
		Unwrap(await @this);
}
