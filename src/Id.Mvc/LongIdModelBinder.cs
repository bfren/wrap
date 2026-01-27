// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Ids;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder for <see cref="long"/> value types.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
public sealed class LongIdModelBinder<TId> : IdModelBinder<TId, long>
	where TId : LongId<TId>, new()
{
	/// <inheritdoc/>
	internal override long Default =>
		0L;

	/// <inheritdoc/>
	internal override Maybe<long> Parse(string? input) =>
		M.ParseInt64(input);
}
