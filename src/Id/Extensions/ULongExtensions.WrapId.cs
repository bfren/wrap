// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class ULongExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this ulong @this)
		where TId : ULongId<TId>, IULongId<TId>, new() =>
		I.Wrap<TId, ulong>(@this);
}
