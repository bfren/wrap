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
	/// Format a string like <see cref="string.Format(IFormatProvider, string, object?[])"/>, with named as well as numbered placeholders.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When <paramref name="source"/> is an array and the placeholders are named, the values are inserted in order,
	/// whatever the names. When it is any other object, each named placeholder takes the value of the public property
	/// with that name (ignoring case). A placeholder with no matching value is left in place.
	/// </para>
	/// <para>
	/// Format specifiers (<c>{A:0.00}</c>) use <see cref="DefaultCulture"/>. Braces outside placeholders are kept as written,
	/// and <c>{{name}}</c> is literal text. Alignment (<c>{A,3}</c>) and mixed numbered and named placeholders are not supported.
	/// This method never throws: unsupported or invalid input returns <paramref name="formatString"/> unchanged.
	/// </para>
	/// <para>
	/// Inspired by https://james.newtonking.com/archive/2008/03/28/formatwith-2-0-string-formatting-with-named-variables/,
	/// (significantly) altered to work without requiring DataBinder.
	/// </para>
	/// </remarks>
	/// <typeparam name="T">Source type.</typeparam>
	/// <param name="formatString">String to format.</param>
	/// <param name="source">Array of values, or object whose properties hold the values.</param>
	/// <param name="replaceIfNullOrEmpty">Value to return if <paramref name="formatString"/> is <see langword="null"/> or white space,
	/// or <paramref name="source"/> is <see langword="null"/> or an empty array.</param>
	/// <returns>
	/// The formatted string; <paramref name="replaceIfNullOrEmpty"/> if the inputs are empty (or, when it is <see langword="null"/>,
	/// <see cref="string.Empty"/> for an empty <paramref name="formatString"/> and <paramref name="formatString"/> for an empty
	/// <paramref name="source"/>); or <paramref name="formatString"/> unchanged if it cannot be formatted.
	/// </returns>
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
	/// Check the inputs, and try <see cref="string.Format(IFormatProvider, string, object?[])"/> first when it can do the job (much faster).
	/// </summary>
	/// <typeparam name="T">Source type.</typeparam>
	/// <param name="formatString">String to format.</param>
	/// <param name="source">Array of values, or object whose properties hold the values.</param>
	/// <param name="replaceIfNullOrEmpty">Value to return if <paramref name="formatString"/> is <see langword="null"/> or white space,
	/// or <paramref name="source"/> is <see langword="null"/> or an empty array.</param>
	/// <returns>
	/// The value to return early if the inputs are empty or <see cref="string.Format(IFormatProvider, string, object?[])"/> succeeds,
	/// or <see langword="null"/> if <see cref="Format{T}(string, T, string?)"/> should carry on.
	/// </returns>
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
	/// Match a placeholder with its surrounding braces, or failing that a doubled (escaped) brace.
	/// </summary>
	/// <remarks>
	/// Thanks James Newton-King!
	/// </remarks>
	/// <returns>The placeholder regex.</returns>
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
