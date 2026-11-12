// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text;
using Wrap.Logging;

namespace Wrap;

public readonly partial record struct Failure
{
	private static readonly CompositeFormat ContextFormat = CompositeFormat.Parse("{0}.{1}()");

	/// <summary>
	/// Replace the arguments used to fill the placeholders in <see cref="Message"/>.
	/// </summary>
	/// <param name="args">Arguments - if <see langword="null"/>, this failure is returned unchanged.</param>
	/// <returns>A copy of this failure with <paramref name="args"/>.</returns>
	public Failure Arg(params object?[] args) =>
		args switch
		{
			{ } =>
				this with
				{
					Args = args ?? []
				},

			_ =>
				this
		};

	/// <summary>
	/// Set the context to <c>{class}.{function}()</c>.
	/// </summary>
	/// <param name="class">Calling class.</param>
	/// <param name="function">Calling function.</param>
	/// <returns>A copy of this failure with the new context.</returns>
	public Failure Ctx(string @class, string function) =>
		this with
		{
			Context = string.Format(F.DefaultCulture, ContextFormat, @class, function)
		};

	/// <summary>
	/// Set the context to the full name of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">Calling type.</typeparam>
	/// <returns>A copy of this failure with the new context.</returns>
	public Failure Ctx<T>() =>
		this with
		{
			Context = typeof(T).FullName
		};

	/// <summary>
	/// Set the log level.
	/// </summary>
	/// <param name="level">Log level.</param>
	/// <returns>A copy of this failure with <paramref name="level"/>.</returns>
	public Failure Lvl(LogLevel level) =>
		this with
		{
			Level = level
		};

	/// <summary>
	/// Replace the message and its arguments.
	/// </summary>
	/// <param name="message">Failure message - if <see langword="null"/>, this failure is returned unchanged.</param>
	/// <param name="args">[Optional] Arguments to use when <paramref name="message"/> contains placeholders.</param>
	/// <returns>A copy of this failure with <paramref name="message"/> and <paramref name="args"/>.</returns>
	public Failure Msg(string? message, params object?[] args) =>
		message switch
		{
			string =>
				this with
				{
					Message = message,
					Args = args
				},

			_ =>
				this
		};
}
