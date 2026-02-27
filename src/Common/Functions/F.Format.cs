// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Wrap;

public static partial class F
{
	internal static ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> TypeInfoCache =
		new();

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
		var regex = TemplateMatcherRegex();
		var values = new List<object>(regex.Count(formatString));
		var replaceIndex = 0; // keeps track of replace loop so we can match named template values with an array source
		var numberedTemplates = true;
		var rewrittenFormat = regex.Replace(formatString, (m) =>
		{
			// This is the value inside the braces, e.g. "0" in "{0}" or "A" in "{A}"
			// Remove any @ symbols from the start - used by Serilog to denote an object format but breaks the following
			var template = m.Groups["template"].Value.TrimStart('@');
			var templateIsNumber = int.TryParse(template, out var templateNumber);
			numberedTemplates = numberedTemplates && templateIsNumber;

			// Switch on the source type, using variety of methods to get this template's value
			values.Add(source switch
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

				// Nothing matches so put placeholder back
				_ =>
					$"{{{template}}}"
			});

			// Recreate format using zero-based string
			return new string('{', m.Groups["start"].Captures.Count)
				+ (values.Count - 1)
				+ m.Groups["format"].Value
				+ new string('}', m.Groups["end"].Captures.Count);
		});

		// Format string with ordered values
		return string.Format(DefaultCulture, rewrittenFormat, [.. values]);
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

			// Attempt to use string.Format
			try
			{
				return string.Format(DefaultCulture, formatString, [.. arr]);
			}
			catch (Exception)
			{
				// Do nothing - null return will cause Format function to continue
			}
		}

		return null;
	}

	/// <summary>
	/// Thanks James Newton-King!
	/// </summary>
	[GeneratedRegex("(?<start>\\{)+(?<template>[\\w\\.\\[\\]@]+)(?<format>:[^}]+)?(?<end>\\})+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
	private static partial Regex TemplateMatcherRegex();

	private static class PropertyCache<T>
	{
		private static readonly ConcurrentDictionary<string, PropertyInfo?> Cache = new();
		private static readonly BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

		internal static PropertyInfo? GetProperty(string name) =>
			Cache.GetOrAdd(name, static n => typeof(T).GetProperty(n, Flags));
	}
}
