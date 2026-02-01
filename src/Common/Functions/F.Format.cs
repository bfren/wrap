// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Wrap;

public static partial class F
{
	/// <inheritdoc cref="Format{T}(string, T, string?)"/>
	public static string Format<T>(string formatString, T source) =>
		Format(formatString, source, null);

	/// <summary>
	/// Works like string.Format() but with named as well as numbered placeholders.
	/// <para>Source is Array: values will be inserted in order (regardless of placeholder values).</para>
	/// <para>Source is Object: property names must match placeholders or they will be left in place.</para>
	/// </summary>
	/// <remarks>
	/// Inspired by http://james.newtonking.com/archive/2008/03/29/formatwith-2-0-string-formatting-with-named-variables,
	/// (significantly) altered to work without requiring DataBinder.
	/// </remarks>
	/// <typeparam name="T">Source type.</typeparam>
	/// <param name="formatString">String to format.</param>
	/// <param name="source">Source object to use for template values.</param>
	/// <param name="replaceIfNullOrEmpty">Returned if <paramref name="formatString"/> or <paramref name="source"/> are null / empty.</param>
	/// <returns>Formatted string.</returns>
	public static string Format<T>(string formatString, T source, string? replaceIfNullOrEmpty)
	{
		// Return if format string is null or empty
		if (string.IsNullOrWhiteSpace(formatString))
		{
			return replaceIfNullOrEmpty ?? string.Empty;
		}

		// Return if source is null or an empty array
		if (source is null)
		{
			return replaceIfNullOrEmpty ?? formatString;
		}
		else if (source is Array arr && arr.Length == 0)
		{
			return replaceIfNullOrEmpty ?? formatString;
		}

		// Thanks James Newton-King!
		var regex = TemplateMatcherRegex();

		var values = new List<object>();
		var replaceIndex = 0; // keeps track of replace loop so we can match named template values with an array source
		var numberedTemplates = true;
		var rewrittenFormat = regex.Replace(formatString, (m) =>
		{
			var startGroup = m.Groups["start"];
			var templateGroup = m.Groups["template"];
			var formatGroup = m.Groups["format"];
			var endGroup = m.Groups["end"];

			// This is the value inside the braces, e.g. "0" in "{0}" or "A" in "{A}"
			// Remove any @ symbols from the start - used by Serilog to denote an object format
			// but breaks the following
			var template = templateGroup.Value.TrimStart('@');
			var templateIsNumber = int.TryParse(template, out var templateNumber);
			numberedTemplates = numberedTemplates && templateIsNumber;

			// Switch on the source type, using variety of methods to get this template's value
			var flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
			var value = source switch
			{
				// Source array - get specific item in array for numbered template
				Array arr when numberedTemplates && templateNumber < arr.Length && arr.GetValue(templateNumber) is object val =>
					val,

				// Source value - use string value but only for the first template, i.e. {0}
				{ } obj when numberedTemplates && templateNumber == 0 && obj.ToString() is string val =>
					val,

				// Source array - get next item in array for named template
				Array arr when !numberedTemplates && replaceIndex < arr.Length && arr.GetValue(replaceIndex++) is object val =>
					val,

				// Source object - get matching property value for named template
				{ } obj when !numberedTemplates && typeof(T).GetProperty(template, flags)?.GetValue(obj) is object val =>
					val,

				// Nothing matches so put placeholder back
				_ =>
					$"{{{template}}}"
			};

			values.Add(value);

			// Recreate format using zero-based string
			return new string('{', startGroup.Captures.Count)
				+ (values.Count - 1)
				+ formatGroup.Value
				+ new string('}', endGroup.Captures.Count);
		});

		// Format string with ordered values
		var formatted = string.Format(DefaultCulture, rewrittenFormat, [.. values]);

		// If the string still contains any placeholders, return original format string
		return regex.IsMatch(formatted) ? formatString : formatted;
	}

	[GeneratedRegex("(?<start>\\{)+(?<template>[\\w\\.\\[\\]@]+)(?<format>:[^}]+)?(?<end>\\})+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
	private static partial Regex TemplateMatcherRegex();
}
