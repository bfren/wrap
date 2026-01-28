// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class GuidExtensions
{
	/// <inheritdoc cref="F.Wrap{TId, TValue}(TValue)"/>
	public static TId WrapId<TId>(this Guid @this)
		where TId : GuidId<TId>, IGuidId<TId>, new() =>
		F.Wrap<TId, Guid>(@this);
}
