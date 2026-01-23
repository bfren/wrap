// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Wrap.Logging;

namespace Wrap;

/// <summary>
/// Fluently create a <see cref="FailValue"/> object.
/// </summary>
public sealed class FluentFailure
{
	private static readonly CompositeFormat ContextFormat = CompositeFormat.Parse("{0}.{1}()");

	internal FailValue Value { get; private set; }

	/// <summary>
	/// Use a pre-existing FailValue.
	/// </summary>
	/// <param name="value">FailValue.</param>
	internal FluentFailure(FailValue value) =>
		Value = value;

	/// <summary>
	/// Fluently create a <see cref="FailValue"/> from a simple failure message.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	internal FluentFailure(string message, object? args = null) =>
		Value = FailValue.Create(message, args);

	/// <summary>
	/// Fluently create a <see cref="Fail"/> object from an exception.
	/// </summary>
	/// <param name="ex">Exception object.</param>
	internal FluentFailure(Exception ex) =>
		Value = FailValue.Create(ex);

	/// <summary>
	/// Add arguments to the failure value.
	/// </summary>
	/// <param name="args">Arguments.</param>
	/// <returns>FluentFail.</returns>
	public FluentFailure Arg<T>(T args)
	{
		Value = Value with
		{
			Args = args
		};

		return this;
	}

	/// <summary>
	/// Add context to the failure value.
	/// </summary>
	/// <param name="class">Calling class.</param>
	/// <param name="function">Calling function.</param>
	/// <returns>FluentFail.</returns>
	public FluentFailure Ctx(string @class, string function)
	{
		Value = Value with
		{
			Context = string.Format(CultureInfo.InvariantCulture, ContextFormat, @class, function)
		};

		return this;
	}

	/// <summary>
	/// Add context to the failure value.
	/// </summary>
	/// <typeparam name="T">Calling type.</typeparam>
	/// <returns>FluentFail.</returns>
	public FluentFailure Ctx<T>()
	{
		Value = Value with
		{
			Context = typeof(T).FullName
		};

		return this;
	}

	/// <summary>
	/// Alter the log level of the failure value.
	/// </summary>
	/// <param name="level">LogLevel.</param>
	/// <returns>FluentFail.</returns>
	public FluentFailure Lvl(LogLevel level)
	{
		Value = Value with
		{
			Level = level
		};

		return this;
	}

	/// <summary>
	/// Add a message to the failure value.
	/// </summary>
	/// <param name="message">Failure message.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>FluentFail.</returns>
	public FluentFailure Msg(string? message, params object?[] args)
	{
		Value = message switch
		{
			string =>
				Value with
				{
					Message = message,
					Args = args
				},

			_ =>
				Value
		};

		return this;
	}
	/// <summary>
	/// Wrap <see cref="Value"/> as a Task for simple async returns.
	/// </summary>
	/// <typeparam name="T">Ok value type.</typeparam>
	/// <returns>Result task.</returns>
	public Task<Result<T>> AsTask<T>() =>
		Result<T>.Failure.Create(Value).AsTask();
}
