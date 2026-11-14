// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Concurrent;
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
	/// Inspired by https://james.newtonking.com/archive/2008/03/28/formatwith-2-0-string-formatting-with-named-variables/,
	/// (significantly) altered to work without requiring DataBinder.
	/// </remarks>
	/// <typeparam name="T">Source type.</typeparam>
	/// <param name="formatString">String to format.</param>
	/// <param name="source">Source object to use for template values.</param>
	/// <param name="replaceIfNullOrEmpty">Returned if <paramref name="formatString"/> or <paramref name="source"/> are null / empty.</param>
	/// <returns>Formatted string.</returns>
	public static string Format<T>(string formatString, T source, string? replaceIfNullOrEmpty)
	{
		// Check arguments before proceeding
		if (Check(formatString, source, replaceIfNullOrEmpty) is string earlyReturn)
		{
			return earlyReturn;
		}

		// Initialise variables before regex replace loop
		var replaceIndex = 0; // keeps track of replace loop so we can match named template values with an array source
		var numberedTemplates = HasOnlyNumberedTemplates(formatString);

		// Write the final text directly: passing the template on to string.Format would make it read any
		// brace this regex does not recognise, e.g. '{max=5}', and throw or use the wrong value
		try
		{
			return TemplateMatcherRegex().Replace(formatString, m =>
			{
				// A doubled brace outside a placeholder is an escaped brace, as in string.Format
				if (!m.Groups["template"].Success)
				{
					return m.Value[..1];
				}

				// Each pair of braces is one escaped brace, so '{{name}}' is text and '{{{name}}}' is a placeholder inside braces
				var (start, end) = (m.Groups["start"].Captures.Count, m.Groups["end"].Captures.Count);
				var (before, after) = (new string('{', start / 2), new string('}', end / 2));
				if (start % 2 != end % 2)
				{
					throw new FormatException("Unbalanced braces.");
				}

				if (start % 2 == 0)
				{
					return before + m.Groups["template"].Value + m.Groups["format"].Value + after;
				}

				// This is the value inside the braces, e.g. "0" in "{0}" or "A" in "{A}"
				// Remove any @ symbols from the start - used by Serilog to denote an object format but breaks the following
				var template = m.Groups["template"].Value.TrimStart('@');
				_ = int.TryParse(template, out var templateNumber);

				// Switch on the source type, using variety of methods to get this template's value
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
					{ } obj when !numberedTemplates && PropertyCache<T>.GetProperty(template)?.GetValue(obj) is object val =>
						val,

					// Nothing matches
					_ =>
						null
				};

				// Apply any format (the text after ':') the way string.Format would, or put the placeholder back
				var format = m.Groups["format"].Success ? m.Groups["format"].Value[1..] : null;
				return before + value switch
				{
					IFormattable x =>
						x.ToString(format, DefaultCulture),

					{ } x =>
						x.ToString(),

					null =>
						$"{{{template}}}"
				} + after;
			});
		}
		catch (FormatException)
		{
			// Unsupported or invalid input, e.g. an unknown format specifier: formatting a message must never throw
			return formatString;
		}
	}

	/// <summary>
	/// Check inputs before attempting to apply complex string format.
	/// </summary>
	/// <typeparam name="T">Source type.</typeparam>
	/// <param name="formatString">String to format.</param>
	/// <param name="source">Source object to use for template values.</param>
	/// <param name="replaceIfNullOrEmpty">Returned if <paramref name="formatString"/> or <paramref name="source"/> are null / empty.</param>
	/// <returns>Early return value if checks fail, or string.Format succeeds (much faster!).</returns>
	internal static string? Check<T>(string formatString, T source, string? replaceIfNullOrEmpty)
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
		else if (source is Array arr)
		{
			if (arr.Length == 0)
			{
				return replaceIfNullOrEmpty ?? formatString;
			}

			// Attempt to use string.Format - named templates always make it throw, so skip it for them
			if (HasOnlyNumberedTemplates(formatString))
			{
				try
				{
					return string.Format(DefaultCulture, formatString, [.. arr]);
				}
				catch (Exception)
				{
					// Do nothing - null return will cause Format function to continue
				}
			}
		}

		return null;
	}

	private static bool HasOnlyNumberedTemplates(string formatString)
	{
		foreach (Match m in TemplateMatcherRegex().Matches(formatString))
		{
			// skip escaped braces, e.g. '{{name}}', which are text and not placeholders
			if (m.Groups["template"].Success && m.Groups["start"].Captures.Count % 2 == 1 && !int.TryParse(m.Groups["template"].Value.TrimStart('@'), out _))
			{
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Thanks James Newton-King!
	/// </summary>
	/// <remarks>
	/// Matches a placeholder with its surrounding braces, or failing that a doubled (escaped) brace.
	/// </remarks>
	[GeneratedRegex("(?<start>\\{)+(?<template>[\\w\\.\\[\\]@]+)(?<format>:[^}]+)?(?<end>\\})+|\\{\\{|\\}\\}", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
	private static partial Regex TemplateMatcherRegex();

	private static class PropertyCache<T>
	{
		private static readonly ConcurrentDictionary<string, PropertyInfo?> Cache = new();
		private static readonly BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

		internal static PropertyInfo? GetProperty(string name) =>
			Cache.GetOrAdd(name, static n => typeof(T).GetProperty(n, Flags));
	}
}
