// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Ids;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder for <see cref="Guid"/> value types.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
public sealed class GuidIdModelBinder<TId> : IdModelBinder<TId, Guid>
	where TId : GuidId<TId>, new()
{
	/// <inheritdoc/>
	internal override Guid Default =>
		Guid.Empty;

	/// <inheritdoc/>
	internal override Maybe<Guid> Parse(string? input) =>
		M.ParseGuid(input);
}
