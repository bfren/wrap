// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder for <see cref="int"/> value types.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
public sealed class IntIdModelBinder<TId> : IdModelBinder<TId, int>
	where TId : IntId<TId>, new()
{
	/// <inheritdoc/>
	internal override int Default =>
		0;

	/// <inheritdoc/>
	internal override Maybe<int> Parse(string? input) =>
		M.ParseInt32(input);
}
