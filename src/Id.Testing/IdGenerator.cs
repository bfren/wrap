// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Testing;

/// <summary>
/// Generate strongly typed IDs with random values, for tests.
/// </summary>
public static class IdGenerator
{
	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="GuidId{TId}"/> type.</typeparam>
	/// <returns>A new <typeparamref name="TId"/> holding a random <see cref="System.Guid"/>.</returns>
	public static TId GuidId<TId>()
		where TId : GuidId<TId>, new() =>
		new() { Value = Rnd.Guid };

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random non-negative value.
	/// </summary>
	/// <typeparam name="TId"><see cref="IntId{TId}"/> type.</typeparam>
	/// <returns>A new <typeparamref name="TId"/> holding a random non-negative value.</returns>
	public static TId IntId<TId>()
		where TId : IntId<TId>, new() =>
		IntId<TId>(true);

	/// <inheritdoc cref="IntId{TId}()"/>
	/// <param name="limit">Intended to limit the value to a small range; both settings currently return a value from 0 to <see cref="int.MaxValue"/>.</param>
	public static TId IntId<TId>(bool limit)
		where TId : IntId<TId>, new() =>
		new() { Value = limit ? Rnd.Int : Rnd.NumberF.GetInt32() };

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random non-negative value.
	/// </summary>
	/// <typeparam name="TId"><see cref="LongId{TId}"/> type.</typeparam>
	/// <returns>A new <typeparamref name="TId"/> holding a random non-negative value.</returns>
	public static TId LongId<TId>()
		where TId : LongId<TId>, new() =>
		LongId<TId>(true);

	/// <inheritdoc cref="LongId{TId}()"/>
	/// <param name="limit">Intended to limit the value to a small range; both settings currently return a value from 0 to <see cref="long.MaxValue"/>.</param>
	public static TId LongId<TId>(bool limit)
		where TId : LongId<TId>, new() =>
		new() { Value = limit ? Rnd.Lng : Rnd.NumberF.GetInt64() };

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="UIntId{TId}"/> type.</typeparam>
	/// <returns>A new <typeparamref name="TId"/> holding a random value.</returns>
	public static TId UIntId<TId>()
		where TId : UIntId<TId>, new() =>
		UIntId<TId>(true);

	/// <inheritdoc cref="UIntId{TId}()"/>
	/// <param name="limit">Intended to limit the value to a small range; both settings currently return a value from 0 to <see cref="uint.MaxValue"/>.</param>
	public static TId UIntId<TId>(bool limit)
		where TId : UIntId<TId>, new() =>
		new() { Value = limit ? Rnd.UInt32 : Rnd.NumberF.GetUInt32() };

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="ULongId{TId}"/> type.</typeparam>
	/// <returns>A new <typeparamref name="TId"/> holding a random value.</returns>
	public static TId ULongId<TId>()
		where TId : ULongId<TId>, new() =>
		ULongId<TId>(true);

	/// <inheritdoc cref="ULongId{TId}()"/>
	/// <param name="limit">Intended to limit the value to a small range; both settings currently return a value from 0 to <see cref="ulong.MaxValue"/>.</param>
	public static TId ULongId<TId>(bool limit)
		where TId : ULongId<TId>, new() =>
		new() { Value = limit ? Rnd.UInt64 : Rnd.NumberF.GetUInt64() };
}
