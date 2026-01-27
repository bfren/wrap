// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder for <see cref="ulong"/> value types.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
public sealed class ULongIdModelBinder<TId> : IdModelBinder<TId, ulong>
	where TId : ULongId<TId>, new()
{
	/// <inheritdoc/>
	internal override ulong Default =>
		0UL;

	/// <inheritdoc/>
	internal override Maybe<ulong> Parse(string? input) =>
		M.ParseUInt64(input);
}
