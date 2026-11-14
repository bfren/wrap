// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder for single-case monads such as strongly typed IDs (<see cref="IMonad{TMonad, TValue}"/>).
/// </summary>
/// <typeparam name="TMonad">Monad type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public sealed class MonadModelBinder<TMonad, TValue> : WrapModelBinder<TValue>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <summary>
	/// Wrap a parsed value in a new <typeparamref name="TMonad"/>.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A new <typeparamref name="TMonad"/> holding <paramref name="value"/>.</returns>
	/// <exception cref="Exceptions.NullMonadValueException"><paramref name="value"/> is <see langword="null"/> and <typeparamref name="TValue"/> is not a nullable value type.</exception>
	public override object Wrap(TValue value) =>
		F.Wrap<TMonad, TValue>(value);
}
