// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class UInt64Extensions
{
	/// <inheritdoc cref="F.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this ulong @this)
		where TId : ULongId<TId>, IULongId<TId>, new() =>
		F.Wrap<TId, ulong>(@this);
}
