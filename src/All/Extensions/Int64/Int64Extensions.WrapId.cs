// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class Int64Extensions
{
	/// <inheritdoc cref="F.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this long @this)
		where TId : LongId<TId>, ILongId<TId>, new() =>
		F.Wrap<TId, long>(@this);
}
