// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Wrap.Exceptions;

namespace Wrap;

/// <summary>
/// Single-case monad that wraps a value of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public sealed record class Monad<T> : Monad<Monad<T>, T>;

/// <summary>
/// Base record for single-case monads that wrap a value.
/// </summary>
/// <typeparam name="TMonad">Monad implementation type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public abstract record class Monad<TMonad, TValue> : IMonad<TMonad, TValue>
	where TMonad : IMonad<TMonad, TValue>, new()
{
	/// <summary>
	/// Gets or initialises the wrapped value.
	/// </summary>
	/// <exception cref="NullMonadValueException">The value is read before it is set, or set to <see langword="null"/>,
	/// and <typeparamref name="TValue"/> is not a nullable value type.</exception>
	[MemberNotNull]
	public TValue Value
	{
		get => Check(wrapped);
		init => wrapped = Check(value);
	}

	// a named field, not the `field` keyword: PrintMembers must read the value without the null check
	private readonly TValue? wrapped;

	/// <summary>
	/// Write the wrapped value for <see cref="object.ToString"/>.
	/// </summary>
	/// <remarks>
	/// Reads the stored value directly, so <see cref="object.ToString"/> does not throw when <see cref="Value"/> has not been set.
	/// </remarks>
	/// <param name="builder">String builder to write to.</param>
	/// <returns><see langword="true"/>, because a member is always written.</returns>
	protected virtual bool PrintMembers(StringBuilder builder)
	{
		_ = builder.Append(nameof(Value)).Append(" = ").Append(Convert.ToString(wrapped, F.DefaultCulture));
		return true;
	}

	/// <summary>
	/// Create the monad without a value, to be set with an initialiser.
	/// </summary>
	protected Monad() { }

	/// <summary>
	/// Create the monad with a value, so derived records can set <see cref="Value"/> on construction.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <exception cref="NullMonadValueException"><paramref name="value"/> is <see langword="null"/> and <typeparamref name="TValue"/> is not a nullable value type.</exception>
	protected Monad([DisallowNull] TValue value) =>
		Value = value;

	/// <summary>
	/// Check that <paramref name="value"/> is not <see langword="null"/>, unless <typeparamref name="TValue"/> is a nullable value type.
	/// </summary>
	/// <param name="value">Value to check.</param>
	/// <returns><paramref name="value"/> unchanged.</returns>
	/// <exception cref="NullMonadValueException"><paramref name="value"/> is <see langword="null"/> and <typeparamref name="TValue"/> is not a nullable value type.</exception>
	private static TValue Check(TValue? value) =>
		value switch
		{
			TValue =>
				value,

			_ when F.IsNullableValueType(value) =>
				value!,

			_ =>
				throw new NullMonadValueException()
		};

	/// <summary>
	/// Wrap a value in a new <typeparamref name="TMonad"/>.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>A new <typeparamref name="TMonad"/> holding <paramref name="value"/>.</returns>
	/// <exception cref="NullMonadValueException"><paramref name="value"/> is <see langword="null"/> and <typeparamref name="TValue"/> is not a nullable value type.</exception>
#pragma warning disable CA1000 // Do not declare static members on generic types
	public static TMonad Wrap(TValue value) =>
		F.Wrap<TMonad, TValue>(value);
#pragma warning restore CA1000 // Do not declare static members on generic types
}
