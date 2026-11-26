// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json.Serialization;
using Wrap.Json;

namespace Wrap;

/// <summary>
/// What sort of failure a <see cref="Failure"/> is, in terms that make sense outside any one transport -
/// so an HTTP framework can map it to a status code, a CLI to an exit code and a UI to a message.
/// </summary>
/// <remarks>
/// <para>
/// <c>default(FailureKind)</c> is <see cref="Unspecified"/>. Kinds compare by <see cref="Name"/> (ordinal, case-sensitive),
/// and are written to JSON as the name string.
/// </para>
/// <para>
/// This is an open set: define your own kinds as static members of your own types. Prefix their names with your library or
/// app name so they cannot clash with someone else's, using a constant so the name also works in <see cref="FailureAttribute"/>
/// and <c>case</c> labels:
/// <code>
/// public const string PayloadTooLargeName = $"{nameof(Merlin)}.{nameof(PayloadTooLarge)}";
/// public static readonly FailureKind PayloadTooLarge = new(PayloadTooLargeName);
/// </code>
/// </para>
/// </remarks>
/// <param name="Name">Kind name, or <see langword="null"/> for <see cref="Unspecified"/>.</param>
[JsonConverter(typeof(FailureKindJsonConverter))]
public readonly record struct FailureKind(string? Name)
{
	/// <summary>
	/// Gets the default kind, used when nothing has been said about the failure.
	/// </summary>
	public static readonly FailureKind Unspecified;

	/// <summary>
	/// Gets the kind for wrong input: retrying with the same input will not help.
	/// </summary>
	public static readonly FailureKind Invalid =
		new(nameof(Invalid));

	/// <summary>
	/// Gets the kind for when the thing asked for does not exist.
	/// </summary>
	public static readonly FailureKind NotFound =
		new(nameof(NotFound));

	/// <summary>
	/// Gets the kind for when the thing already exists, or its state has moved on.
	/// </summary>
	public static readonly FailureKind Conflict =
		new(nameof(Conflict));

	/// <summary>
	/// Gets the kind for when the caller is not identified.
	/// </summary>
	public static readonly FailureKind Unauthenticated =
		new(nameof(Unauthenticated));

	/// <summary>
	/// Gets the kind for when the caller is known but not allowed.
	/// </summary>
	public static readonly FailureKind Forbidden =
		new(nameof(Forbidden));

	/// <summary>
	/// Gets the kind for when a dependency is down: retrying later may work.
	/// </summary>
	public static readonly FailureKind Unavailable =
		new(nameof(Unavailable));

	/// <summary>
	/// Gets the kind for when the operation took too long.
	/// </summary>
	public static readonly FailureKind Timeout =
		new(nameof(Timeout));

	/// <summary>
	/// Gets the kind for when the caller gave up.
	/// </summary>
	public static readonly FailureKind Cancelled =
		new(nameof(Cancelled));

	/// <summary>
	/// Gets the kind for a bug or an unhandled exception.
	/// </summary>
	public static readonly FailureKind Unexpected =
		new(nameof(Unexpected));

	/// <summary>
	/// Get the kind that matches an exception.
	/// </summary>
	/// <param name="ex">Exception object.</param>
	/// <returns>
	/// <see cref="Timeout"/> for a <see cref="TimeoutException"/>, or a cancellation caused by one (how <c>HttpClient</c>
	/// reports a timeout), <see cref="Cancelled"/> for any other <see cref="OperationCanceledException"/>,
	/// and <see cref="Unexpected"/> for everything else.
	/// </returns>
	/// <remarks>
	/// An <see cref="AggregateException"/> holding exactly one exception, which is what <c>task.Result</c> and
	/// <c>task.Wait()</c> throw, gets the kind of that exception, even when aggregates are nested. One holding several
	/// exceptions is <see cref="Unexpected"/>, because no single kind fits.
	/// </remarks>
	internal static FailureKind From(Exception ex) =>
		ex switch
		{
			AggregateException aggregate when aggregate.Flatten().InnerExceptions is [var inner] =>
				From(inner),

			TimeoutException or OperationCanceledException { InnerException: TimeoutException } =>
				Timeout,

			OperationCanceledException =>
				Cancelled,

			_ =>
				Unexpected
		};

	/// <summary>
	/// Convert this kind to a string.
	/// </summary>
	/// <returns><see cref="Name"/>, or an empty string for <see cref="Unspecified"/>.</returns>
	public override string ToString() =>
		Name ?? string.Empty;
}
