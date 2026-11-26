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
	/// Output a text input holding the value of a <see cref="Maybe{T}"/> property, or nothing when it is <see cref="None"/>.
	/// </summary>
	/// <remarks>
	/// The value (or for a monad such as an ID, its <see cref="IMonad.Value"/>) is written using <see cref="F.DefaultCulture"/>, the culture <see cref="MaybeModelBinder{T}"/> parses with,
	/// so it binds back to the same value. The input is empty for <see cref="None"/> or when the view has no model, and
	/// an empty input binds back as <see cref="None"/>. As with any MVC text box, a value in ModelState (after a failed
	/// POST) is shown instead.
	/// </remarks>
	/// <typeparam name="TModel">Model type.</typeparam>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <param name="this">HTML helper.</param>
	/// <param name="expression">Expression that returns the <see cref="Maybe{T}"/> property, also used to name the input.</param>
	/// <param name="htmlAttributes">[Optional] HTML attributes for the input, for example <c>new { @class = "form-control" }</c>.</param>
	/// <returns>The text input element.</returns>
	public static IHtmlContent TextBoxForMaybe<TModel, T>(
		this IHtmlHelper<TModel> @this,
		Expression<Func<TModel, Maybe<T>>> expression,
		object? htmlAttributes = null
	)
	{
		// Use helper to generate input name
		var name = @this.NameFor(expression);

		// Get the value, or an empty string for None
		var value = @this.ViewData.Model switch
		{
			// a monad prints as its record (Postcode { Value = … }), so write its inner value, which the binder reads back
			TModel model when GetMonad(expression, model) is Some<T> x =>
				Convert.ToString(x.Value is IMonad m ? m.Value : x.Value, F.DefaultCulture) ?? string.Empty,

			_ =>
				string.Empty
		};

		// Return text input
		return @this.TextBox(name, value, null, htmlAttributes);
	}
}
