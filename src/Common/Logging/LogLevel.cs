// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Logging;

/// <summary>
/// Log Levels - descriptions taken from https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loglevel?view=dotnet-plat-ext-5.0
/// </summary>
public enum LogLevel
{
	/// <summary>
	/// Unknown log level - will use default.
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// Logs that contain the most detailed messages.
	/// These messages may contain sensitive application data.
	/// These messages are disabled by default and should never be enabled in a production environment.
	/// </summary>
	Verbose = 1 << 0,

	/// <summary>
	/// Logs that are used for interactive investigation during development.
	/// These logs should primarily contain information useful for debugging and have no long-term value.
	/// </summary>
	Debug = 1 << 1,

	/// <summary>
	/// Logs that track the general flow of the application. These logs should have long-term value.
	/// </summary>
	Information = 1 << 2,

	/// <summary>
	/// Logs that highlight an abnormal or unexpected event in the application flow,
	/// but do not otherwise cause the application execution to stop.
	/// </summary>
	Warning = 1 << 3,

	/// <summary>
	/// Logs that highlight when the current flow of execution is stopped due to a failure.
	/// These should indicate a failure in the current activity, not an application-wide failure.
	/// </summary>
	Error = 1 << 4,

	/// <summary>
	/// Logs that describe an unrecoverable application or system crash,
	/// or a catastrophic failure that requires immediate attention.
	/// </summary>
	Fatal = 1 << 5
}
