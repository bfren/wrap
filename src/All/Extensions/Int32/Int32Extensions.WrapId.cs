// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Extensions;

public static partial class Int32Extensions
{
	/// <summary>
	/// Wrap a value in a new strongly typed ID.
	/// </summary>
	/// <typeparam name="TId"><see cref="IntId{TId}"/> implementation type.</typeparam>
	/// <param name="this">ID value.</param>
	/// <returns>A new <typeparamref name="TId"/> holding <paramref name="this"/>.</returns>
	public static TId WrapId<TId>(this int @this)
		where TId : IntId<TId>, IIntId<TId>, new() =>
		F.Wrap<TId, int>(@this);
}
