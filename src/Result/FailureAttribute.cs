// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Wrap.Logging;

namespace Wrap;

// In a shared/attributes project
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class FailureAttribute : Attribute
{
	public string Message { get; private init; }

	public LogLevel Level { get; private init; }

	public FailureAttribute(string message, LogLevel level = FailureValue.DefaultFailureLevel) =>
		(Message, Level) = (message, level);
}
