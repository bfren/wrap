// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Wrap.Logging;
using MsLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Wrap.Extensions;

public static partial class LogLevelExtensions
{
	/// <summary>
	/// Convert a Wrap log level to the equivalent Microsoft log level.
	/// </summary>
	/// <remarks>
	/// Wrap's levels are bit flags and Microsoft's are sequential, so the values must be mapped, never cast.
	/// Unknown levels map to <see cref="MsLevel.Warning"/>.
	/// </remarks>
	/// <param name="this">Wrap log level.</param>
	/// <returns>The equivalent Microsoft log level.</returns>
	public static MsLevel ToMsLevel(this LogLevel @this) =>
		@this switch
		{
			LogLevel.Verbose =>
				MsLevel.Trace,

			LogLevel.Debug =>
				MsLevel.Debug,

			LogLevel.Information =>
				MsLevel.Information,

			LogLevel.Warning =>
				MsLevel.Warning,

			LogLevel.Error =>
				MsLevel.Error,

			LogLevel.Fatal =>
				MsLevel.Critical,

			LogLevel.Unknown =>
				MsLevel.Warning,

			_ =>
				MsLevel.Warning
		};
}
