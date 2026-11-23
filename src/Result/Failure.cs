// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// The 'Failure' case of <see cref="Result{T}"/>, which holds information about why an operation failed (see <seealso cref="Ok{T}"/>).
/// </summary>
/// <remarks>
/// <see cref="Failure"/> carries no type, so it converts to any <see cref="Result{T}"/> - you can
/// <c>return R.Fail("…");</c> without specifying the value type. Use <see cref="R.Fail(string, object[])"/> to create one,
/// and <see cref="Msg"/>, <see cref="Arg"/>, <see cref="Ctx(string, string)"/>, <see cref="Lvl"/> and <see cref="Knd"/> to change it.
/// </remarks>
public readonly partial record struct Failure : IWithException
{
	/// <summary>
	/// Gets the failure held by <c>default(Result&lt;T&gt;)</c>.
	/// </summary>
	internal static Failure Default =>
		new("Uninitialised Failure", [])
		{
			Level = DefaultFailureLevel
		};

	/// <summary>
	/// Gets the failure used when an <see cref="Ok{T}"/> holding <see langword="null"/> is converted to <see cref="Result{T}"/>.
	/// </summary>
	internal static Failure NullValue =>
		new("Null Result value - try using Maybe<T> to wrap null values safely.", [])
		{
			Level = DefaultFailureLevel
		};

	internal const LogLevel DefaultFailureLevel =
		LogLevel.Warning;

	internal const LogLevel DefaultExceptionLevel =
		LogLevel.Error;

	/// <summary>
	/// Gets the arguments used to fill the placeholders in <see cref="Message"/> (empty if there are none).
	/// </summary>
	public readonly object?[] Args { get; init; } =
		[];

	/// <summary>
	/// Gets the context of the failure, usually the calling type or method, or <see langword="null"/> if none was set.
	/// </summary>
	public string? Context { get; init; }

	/// <summary>
	/// Gets the exception that caused the failure, or <see langword="null"/> if there was none.
	/// </summary>
	/// <remarks>
	/// Left out of JSON: <c>System.Text.Json</c> cannot write an exception that has been thrown, and its details should not
	/// reach clients. <see cref="Result{T}"/> can be written to JSON but not read back.
	/// </remarks>
	[JsonIgnore]
	public readonly Exception? Exception { get; init; }

	/// <summary>
	/// Gets what sort of failure this is: <see cref="FailureKind.Unspecified"/> by default, or chosen from the exception
	/// when it was created from one.
	/// </summary>
	public readonly FailureKind Kind { get; init; }

	/// <summary>
	/// Gets the log level of the failure: <see cref="LogLevel.Warning"/> by default, or <see cref="LogLevel.Error"/>
	/// when it was created from an exception.
	/// </summary>
	public readonly required LogLevel Level { get; init; }

	/// <summary>
	/// Gets the failure message, which may contain placeholders filled from <see cref="Args"/>.
	/// </summary>
	public readonly required string Message { get; init; }

	/// <summary>
	/// Create a <see cref="Failure"/> from a message, with the <see cref="LogLevel.Warning"/> log level.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	[SetsRequiredMembers]
	public Failure(string message, params object?[] args) =>
		(Message, Args, Level) = (message, args, DefaultFailureLevel);

	/// <summary>
	/// Create a <see cref="Failure"/> from an exception, using its message and the <see cref="LogLevel.Error"/> log level.
	/// </summary>
	/// <remarks>
	/// <see cref="Kind"/> is <see cref="FailureKind.Timeout"/> for a <see cref="TimeoutException"/> or a cancellation caused by
	/// one, <see cref="FailureKind.Cancelled"/> for any other <see cref="OperationCanceledException"/>, and
	/// <see cref="FailureKind.Unexpected"/> otherwise.
	/// </remarks>
	/// <param name="ex">Exception object.</param>
	[SetsRequiredMembers]
	public Failure(Exception ex) =>
		(Message, Exception, Level, Kind) = (ex.Message, ex, DefaultExceptionLevel, FailureKind.From(ex));

	/// <summary>
	/// Wrap this failure as a <see cref="Result{T}"/> in a completed task, for simple async returns.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <returns>A completed task holding this failure as a <see cref="Result{T}"/>.</returns>
	public Task<Result<T>> AsTask<T>() =>
		Task.FromResult((Result<T>)this);

	/// <summary>
	/// Convert this failure to a string.
	/// </summary>
	/// <returns><see cref="Message"/> with its placeholders filled from <see cref="Args"/>.</returns>
	public override string ToString() =>
		F.Format(Message, Args);
}
