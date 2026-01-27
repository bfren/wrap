// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class UIntExtensions
{
	/// <inheritdoc cref="I.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this uint @this)
		where TId : UIntId<TId>, IUintId<TId>, new() =>
		I.Wrap<TId, uint>(@this);
}
