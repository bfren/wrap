// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Wrap;

/// <summary>
/// Result monad: a union that holds either an <see cref="Ok{T}"/> value or a <see cref="Failure"/>.
/// </summary>
/// <remarks>
/// <para>
/// Use pattern matching to read the value - the compiler checks that both cases are handled:
/// </para>
/// <code>
/// var text = result switch
/// {
///     Ok&lt;int&gt; x => $"Value: {x.Value}",
///     Failure f => f.Message
/// };
/// </code>
/// <para>
/// This is a value type: <c>default(Result&lt;T&gt;)</c> is a <see cref="Failure"/> with the message
/// <c>Uninitialised Failure</c>, and a <see cref="Result{T}"/> can never be <see langword="null"/>.
/// </para>
/// </remarks>
/// <typeparam name="T">Ok value type.</typeparam>
[Union]
public readonly partial struct Result<T> : IEither<Result<T>, Failure, T>, IEquatable<Result<T>>
{
	/// <summary>
	/// The wrapped value - only meaningful when <see cref="isOk"/> is <see langword="true"/>.
	/// </summary>
	private readonly T? value;

	/// <summary>
	/// The failure - only meaningful when <see cref="isOk"/> is <see langword="false"/>.
	/// Its <see cref="Failure.Message"/> is <see langword="null"/> only when this object is <c>default</c>.
	/// </summary>
	private readonly Failure failure;

	/// <summary>
	/// Whether this object is an <see cref="Ok{T}"/> (<see langword="true"/>) or a <see cref="Failure"/> (<see langword="false"/>).
	/// </summary>
	private readonly bool isOk;

	/// <summary>
	/// Gets <see langword="true"/> always: the union always holds a case, because <c>default</c> is a <see cref="Failure"/>.
	/// </summary>
	/// <remarks>
	/// Called by the compiler for <c>is</c> patterns and <c>switch</c>, so it knows a <see cref="Result{T}"/>
	/// is never <see langword="null"/> and no <see langword="null"/> case is needed.
	/// </remarks>
	public bool HasValue =>
		true;

	/// <summary>
	/// Gets the current case, boxed as <see cref="Ok{T}"/> or <see cref="Failure"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Required by the union pattern and used by reflection and serialisers. The compiler calls it for
	/// patterns that neither <see cref="TryGetValue(out Ok{T})"/> nor <see cref="TryGetValue(out Failure)"/> covers.
	/// It allocates on every call - use pattern matching instead.
	/// </para>
	/// <para>
	/// For <c>default(Result&lt;T&gt;)</c> it returns a <see cref="Failure"/> with the message <c>Uninitialised Failure</c>.
	/// </para>
	/// </remarks>
	public object? Value =>
		isOk switch
		{
			true =>
				new Ok<T>(value!),

			false =>
				failure.Message is null ? Failure.Default : failure
		};

	/// <summary>
	/// Create a <see cref="Result{T}"/> from an <see cref="Ok{T}"/> value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <see cref="Ok{T}.Value"/> is <see langword="null"/> (including an empty <see cref="Nullable{T}"/>) the result
	/// is a <see cref="Failure"/>, so an <see cref="Ok{T}"/> case never holds <see langword="null"/>.
	/// </para>
	/// <para>
	/// The compiler also uses this constructor for the implicit conversion from <see cref="Ok{T}"/>.
	/// </para>
	/// </remarks>
	/// <param name="ok">Ok value.</param>
	public Result(Ok<T> ok)
	{
		if (ok.Value is null)
		{
			(failure, isOk) = (Failure.NullValue, false);
		}
		else
		{
			(value, isOk) = (ok.Value, true);
		}
	}

	/// <summary>
	/// Create a <see cref="Result{T}"/> from a <see cref="Failure"/> value.
	/// </summary>
	/// <remarks>
	/// The compiler also uses this constructor for the implicit conversion from <see cref="Failure"/>.
	/// </remarks>
	/// <param name="failure">Failure value.</param>
	public Result(Failure failure) =>
		(this.failure, isOk) = (failure, false);

	/// <inheritdoc/>
	public Task<Result<T>> AsTask() =>
		Task.FromResult(this);
}
