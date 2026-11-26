// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Globalization;
using System.Numerics;

namespace Wrap.Dapper;

/// <summary>
/// Read integer ID values from database values, for the integer ID type handlers.
/// </summary>
internal static class IntegerIdValue
{
	/// <summary>
	/// Convert a database value to a <typeparamref name="T"/>, if it is a whole number that fits.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Integer types are converted directly, without the allocation and culture of <see cref="object.ToString()"/>.
	/// A <see langword="decimal"/> is accepted if its value is whole, whatever its scale, so <c>5m</c>, <c>5.0m</c> and
	/// <c>5.00m</c> all give 5, but <c>5.5m</c> is rejected.
	/// </para>
	/// <para>
	/// Text that holds a whole number is read the same way, using <see cref="F.DefaultCulture"/>, so <c>"42"</c>,
	/// <c>"42.0"</c>, <c>"1,000"</c> and <c>"4.2E1"</c> are accepted, but <c>"42.5"</c> is rejected.
	/// </para>
	/// <para>
	/// Anything else (other text, floating-point numbers and so on) is converted to a string and given to
	/// <paramref name="parse"/>.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">ID value type.</typeparam>
	/// <param name="value">Database value.</param>
	/// <param name="parse">Parses the string form of any other value.</param>
	/// <returns>
	/// <see cref="Some{T}"/> holding the value, or <see cref="None"/> if <paramref name="value"/> is a number that is
	/// out of range or not whole, or <paramref name="parse"/> cannot parse it.
	/// </returns>
	internal static Maybe<T> Convert<T>(object? value, Func<string?, Maybe<T>> parse)
		where T : struct, IBinaryInteger<T>, IMinMaxValue<T> =>
		value switch
		{
			T x =>
				x,

			sbyte x =>
				ConvertIfInRange<T>(x),

			byte x =>
				ConvertIfInRange<T>(x),

			short x =>
				ConvertIfInRange<T>(x),

			ushort x =>
				ConvertIfInRange<T>(x),

			int x =>
				ConvertIfInRange<T>(x),

			uint x =>
				ConvertIfInRange<T>(x),

			long x =>
				ConvertIfInRange<T>(x),

			ulong x =>
				ConvertIfInRange<T>(x),

			// decimal's range (about 7.9e28) fits in Int128, so the cast cannot overflow
			decimal x when x == decimal.Truncate(x) =>
				ConvertIfInRange<T>((Int128)x),

			string s =>
				parse(s) switch
				{
					Some<T> x =>
						x,

					None =>
						ConvertWholeText<T>(s)
				},

			null =>
				parse(null),

			_ =>
				parse(System.Convert.ToString(value, F.DefaultCulture))
		};

	/// <summary>
	/// Read text that <c>parse</c> rejected as a whole number, such as <c>"42.0"</c>, <c>"1,000"</c> or <c>"4.2E1"</c>.
	/// </summary>
	/// <remarks>
	/// It uses the same number styles that <see cref="IdTypeHandler{TId, TIdValue}"/> uses to decide that text holds a
	/// number, so text read as a number here is never then rejected there as "not a number", and the reverse.
	/// </remarks>
	/// <typeparam name="T">ID value type.</typeparam>
	/// <param name="value">Text to read.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or <see cref="None"/> if it is not a whole number that fits.</returns>
	private static Maybe<T> ConvertWholeText<T>(string value)
		where T : struct, IBinaryInteger<T>, IMinMaxValue<T> =>
		decimal.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, F.DefaultCulture, out var x) switch
		{
			true when x == decimal.Truncate(x) =>
				ConvertIfInRange<T>((Int128)x),

			_ =>
				M.None
		};

	/// <summary>
	/// Convert <paramref name="value"/> to a <typeparamref name="T"/>, if it fits.
	/// </summary>
	/// <remarks>
	/// <see cref="Int128"/> holds every value of the integer types <see cref="Convert"/> accepts, so the range check
	/// itself cannot overflow.
	/// </remarks>
	/// <typeparam name="T">ID value type.</typeparam>
	/// <param name="value">Whole number to convert.</param>
	/// <returns><see cref="Some{T}"/> holding the value, or <see cref="None"/> if it is out of range for <typeparamref name="T"/>.</returns>
	private static Maybe<T> ConvertIfInRange<T>(Int128 value)
		where T : struct, IBinaryInteger<T>, IMinMaxValue<T> =>
		(value >= Int128.CreateChecked(T.MinValue) && value <= Int128.CreateChecked(T.MaxValue)) switch
		{
			true =>
				T.CreateChecked(value),

			false =>
				M.None
		};
}
