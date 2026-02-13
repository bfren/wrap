// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Monad{TMonad, TValue}"/> MVC model binder.
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public sealed class MonadModelBinder<TMonad, TValue> : WrapModelBinder<TValue>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <inheritdoc/>
	public override object Wrap(TValue value) =>
		F.Wrap<TMonad, TValue>(value);
}
