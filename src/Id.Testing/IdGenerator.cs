// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Testing;

/// <summary>
/// Generate StrongIds with random values.
/// </summary>
public static class IdGenerator
{
	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="IGuidId{TId}"/> type.,</typeparam>
	public static TId GuidId<TId>()
		where TId : GuidId<TId>, new() =>
		new() { Value = Rnd.Guid };

	/// <inheritdoc cref="IntId{TId}(bool)"/>
	public static TId IntId<TId>()
		where TId : IntId<TId>, new() =>
		IntId<TId>(true);

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="IIntId{TId}"/> type.</typeparam>
	/// <param name="limit">If true, ID Value will be limited to 0-10000 - for testing this is all that's needed.</param>
	public static TId IntId<TId>(bool limit)
		where TId : IntId<TId>, new() =>
		new() { Value = limit ? Rnd.Int : Rnd.NumberF.GetInt32() };

	/// <inheritdoc cref="LongId{TId}(bool)"/>
	public static TId LongId<TId>()
		where TId : LongId<TId>, new() =>
		LongId<TId>(true);

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="ILongId{TId}"/> type.</typeparam>
	/// <param name="limit">If true, ID Value will be limited to 0-10000 - for testing this is all that's needed.</param>
	public static TId LongId<TId>(bool limit)
		where TId : LongId<TId>, new() =>
		new() { Value = limit ? Rnd.Lng : Rnd.NumberF.GetInt64() };

	/// <inheritdoc cref="UIntId{TId}(bool)"/>
	public static TId UIntId<TId>()
		where TId : UIntId<TId>, new() =>
		UIntId<TId>(true);

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="IUintId{TId}"/> type.</typeparam>
	/// <param name="limit">If true, ID Value will be limited to 0-10000 - for testing this is all that's needed.</param>
	public static TId UIntId<TId>(bool limit)
		where TId : UIntId<TId>, new() =>
		new() { Value = limit ? Rnd.UInt32 : Rnd.NumberF.GetUInt32() };

	/// <inheritdoc cref="ULongId{TId}(bool)"/>
	public static TId ULongId<TId>()
		where TId : ULongId<TId>, new() =>
		ULongId<TId>(true);

	/// <summary>
	/// Generate a new <typeparamref name="TId"/> with a random value.
	/// </summary>
	/// <typeparam name="TId"><see cref="IULongId{TId}"/> type.</typeparam>
	/// <param name="limit">If true, ID Value will be limited to 0-10000 - for testing this is all that's needed.</param>
	public static TId ULongId<TId>(bool limit)
		where TId : ULongId<TId>, new() =>
		new() { Value = limit ? Rnd.UInt64 : Rnd.NumberF.GetUInt64() };
}
