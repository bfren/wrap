// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder for <see cref="uint"/> value types.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
public sealed class UIntIdModelBinder<TId> : IdModelBinder<TId, uint>
	where TId : UIntId<TId>, new()
{
	/// <inheritdoc/>
	internal override uint Default =>
		0u;

	/// <inheritdoc/>
	internal override Maybe<uint> Parse(string? input) =>
		M.ParseUInt32(input);
}
