// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// 'Failure' Result - holds information why an operation failed -
/// enables implicit operators to handle returning it as <see cref="Result{T}"/>
/// without knowing the generic type.
/// </summary>
public readonly partial struct Failure : IEquatable<Failure>, IUnion<Failure, FailureValue>
{
	private static readonly CompositeFormat ContextFormat = CompositeFormat.Parse("{0}.{1}()");

	/// <summary>
	/// Returns information about the failure.
	/// </summary>
	public readonly required FailureValue Value { get; init; }

	/// <summary>
	/// Use a pre-existing FailureValue.
	/// </summary>
	/// <param name="value">FailureValue.</param>
	[SetsRequiredMembers]
	internal Failure(FailureValue value) =>
		Value = value;

	/// <summary>
	/// Fluently create a <see cref="FailureValue"/> from a simple failure message.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	[SetsRequiredMembers]
	internal Failure(string message, params object?[] args) =>
		Value = new(message, args);

	/// <summary>
	/// Fluently create a <see cref="Failure"/> object from an exception.
	/// </summary>
	/// <param name="ex">Exception object.</param>
	[SetsRequiredMembers]
	internal Failure(Exception ex) =>
		Value = new(ex);

	/// <summary>
	/// Add arguments to the failure value.
	/// </summary>
	/// <param name="args">Arguments.</param>
	/// <returns>FluentFail.</returns>
	public Failure Arg(params object?[] args) =>
		args switch
		{
			{ } =>
				this with
				{
					Value = Value with
					{
						Args = args ?? []
					}
				},

			_ =>
				this
		};

	/// <summary>
	/// Add context to the failure value.
	/// </summary>
	/// <param name="class">Calling class.</param>
	/// <param name="function">Calling function.</param>
	/// <returns>FluentFail.</returns>
	public Failure Ctx(string @class, string function) =>
		this with
		{
			Value = Value with
			{
				Context = string.Format(F.DefaultCulture, ContextFormat, @class, function)
			}
		};

	/// <summary>
	/// Add context to the failure value.
	/// </summary>
	/// <typeparam name="T">Calling type.</typeparam>
	/// <returns>FluentFail.</returns>
	public Failure Ctx<T>() =>
		this with
		{
			Value = Value with
			{
				Context = typeof(T).FullName
			}
		};

	/// <summary>
	/// Alter the log level of the failure value.
	/// </summary>
	/// <param name="level">LogLevel.</param>
	/// <returns>FluentFail.</returns>
	public Failure Lvl(LogLevel level) =>
		this with
		{
			Value = Value with
			{
				Level = level
			}
		};

	/// <summary>
	/// Add a message to the failure value.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>FluentFail.</returns>
	public Failure Msg(string message, params object?[] args) =>
		message switch
		{
			string =>
				this with
				{
					Value = Value with
					{
						Message = message,
						Args = args
					}
				},

			_ =>
				this
		};

	/// <summary>
	/// Create Failure object from <see cref="Value"/> and wrap as a completed Task
	/// for simple async returns.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <returns>Failure task.</returns>
	public Task<Result<T>> AsTask<T>() =>
		new Result<T>.FailureImpl(Value).AsTask();
}
