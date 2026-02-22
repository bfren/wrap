// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class Int32Extensions
{
	/// <inheritdoc cref="F.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this int @this)
		where TId : IntId<TId>, IIntId<TId>, new() =>
		F.Wrap<TId, int>(@this);
}
