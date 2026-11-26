// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Wrap.Mvc;

public static partial class HtmlHelperExtensions
{
	/// <summary>
	/// Output a checkbox for a <see cref="Maybe{T}"/> of <see cref="bool"/> property, ticked only when it is <c>Some(true)</c>.
	/// </summary>
	/// <remarks>
	/// The checkbox is not ticked for <c>Some(false)</c>, <see cref="None"/>, or when the view has no model. Like any MVC
	/// checkbox it also writes a hidden <c>false</c> input, so the form always sends a value: it binds back as
	/// <c>Some(true)</c> or <c>Some(false)</c>, never <see cref="None"/>. Use a select or radio buttons if you need to
	/// keep <see cref="None"/>.
	/// </remarks>
	/// <typeparam name="TModel">Model type.</typeparam>
	/// <param name="this">HTML helper.</param>
	/// <param name="expression">Expression that returns the <see cref="Maybe{T}"/> property, also used to name the input.</param>
	/// <param name="htmlAttributes">[Optional] HTML attributes for the checkbox, for example <c>new { @class = "form-check-input" }</c>.</param>
	/// <returns>The checkbox and hidden input elements.</returns>
	public static IHtmlContent CheckBoxForMaybe<TModel>(
		this IHtmlHelper<TModel> @this,
		Expression<Func<TModel, Maybe<bool>>> expression,
		object? htmlAttributes = null
	)
	{
		// Use helper to generate input name
		var name = @this.NameFor(expression);

		// Tick the checkbox only for Some(true)
		var isChecked = @this.ViewData.Model switch
		{
			TModel model =>
				GetMonad(expression, model) is Some<bool> { Value: true },

			_ =>
				false
		};

		// Return checkbox
		return @this.CheckBox(name, isChecked, htmlAttributes);
	}
}
