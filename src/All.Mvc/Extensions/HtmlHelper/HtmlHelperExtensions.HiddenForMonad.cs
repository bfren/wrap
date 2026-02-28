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
	/// Output a hidden HTML input for a Monad value.
	/// </summary>
	/// <typeparam name="TModel">Model type.</typeparam>
	/// <typeparam name="TMonad">Monad type.</typeparam>
	/// <param name="this">HtmlHelper.</param>
	/// <param name="expression">Expression to return Monad property.</param>
	public static IHtmlContent HiddenForMonad<TModel, TMonad>(this IHtmlHelper<TModel> @this, Expression<Func<TModel, TMonad>> expression)
		where TMonad : IMonad
	{
		// Use helper to generate input name
		var name = @this.NameFor(expression);

		// Get nullable value
		var value = @this.ViewData.Model switch
		{
			TModel model =>
				expression.Compile().Invoke(model).Value.ToString(),

			_ =>
				null
		};

		// Return hidden input
		return @this.Hidden(name, value ?? string.Empty);
	}
}
