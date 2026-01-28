// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class UInt32Extensions
{
	/// <inheritdoc cref="F.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this uint @this)
		where TId : UIntId<TId>, IUintId<TId>, new() =>
		F.Wrap<TId, uint>(@this);
}
