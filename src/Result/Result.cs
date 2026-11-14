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
public readonly partial struct Result<T> : IEither<Failure, T>, IEquatable<Result<T>>
{
	/// <summary>
	/// The wrapped value - only meaningful when <see cref="isOk"/> is <see langword="true"/>.
	/// </summary>
	private readonly T? value;

	/// <summary>
	/// The failure, boxed on the heap so <see cref="Result{T}"/> stays small and cheap to copy.
	/// <see langword="null"/> when this is an <see cref="Ok{T}"/>, and when this object is <c>default</c>.
	/// </summary>
	private readonly StrongBox<Failure>? failure;

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
				failure?.Value ?? Failure.Default
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
			(failure, isOk) = (new(Failure.NullValue), false);
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
	/// <para>
	/// The failure is stored on the heap (one small allocation), so a <see cref="Result{T}"/> stays small
	/// and the <see cref="Ok{T}"/> path stays fast and allocation-free.
	/// </para>
	/// <para>
	/// The compiler also uses this constructor for the implicit conversion from <see cref="Failure"/>.
	/// </para>
	/// </remarks>
	/// <param name="failure">Failure value.</param>
	public Result(Failure failure) =>
		(this.failure, isOk) = (new(failure), false);

	/// <summary>
	/// Create a failed <see cref="Result{T}"/> that shares an existing failure box.
	/// </summary>
	/// <remarks>
	/// A <see langword="null"/> box gives the same failure as <c>default(Result&lt;T&gt;)</c>.
	/// </remarks>
	/// <param name="failure">Failure box to share, or <see langword="null"/>.</param>
	private Result(StrongBox<Failure>? failure) =>
		(this.failure, isOk) = (failure, false);

	/// <summary>
	/// Pass this object's failure on to a <see cref="Result{T}"/> of another type, sharing its box rather than allocating a new one.
	/// </summary>
	/// <remarks>
	/// Only call this when this object is not an <see cref="Ok{T}"/>: for an <see cref="Ok{T}"/> it returns
	/// the same failure as <c>default(Result&lt;TReturn&gt;)</c>.
	/// </remarks>
	/// <typeparam name="TReturn">Ok value type of the returned result.</typeparam>
	/// <returns>A failed <see cref="Result{T}"/> holding the same failure as this object.</returns>
	// without inlining, this cold-path call stops the JIT optimising Bind's Ok path (measured 6.9 ns vs 2.1 ns)
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal Result<TReturn> PassFailure<TReturn>() =>
		new(failure);

	/// <summary>
	/// Get the failure held by this object.
	/// </summary>
	/// <remarks>
	/// Only call this when this object is not an <see cref="Ok{T}"/>: for an <see cref="Ok{T}"/> it returns
	/// the same failure as <c>default(Result&lt;T&gt;)</c>.
	/// </remarks>
	/// <returns>The <see cref="Failure"/> held by this object.</returns>
	// lets a switch use a discard arm instead of `Failure f =>`, which costs every call a 40-byte Failure local
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal Failure GetFailure() =>
		failure?.Value ?? Failure.Default;

	/// <summary>
	/// Wrap this object in a completed task.
	/// </summary>
	/// <returns>A completed task holding this object.</returns>
	public Task<Result<T>> AsTask() =>
		Task.FromResult(this);
}
