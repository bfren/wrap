// Monads: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monads;

public static partial class F
{
	/// <summary>
	/// Static error logger - by default do nothing.
	/// </summary>
	public static ErrorLogger LogError { get; set; } =
		_ => { };

	/// <summary>
	/// Static exception logger - by default do nothing.
	/// </summary>
	public static ExceptionLogger LogException { get; set; } =
		_ => { };
}
