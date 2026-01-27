// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Wrap.Extensions;

public static partial class HtmlHelperExtensions
{
	/// <summary>
	/// Output a hidden HTML input for an ID value.
	/// </summary>
	/// <typeparam name="TModel">Model type.</typeparam>
	/// <typeparam name="TId">ID type.</typeparam>
	/// <param name="this">HtmlHelper.</param>
	/// <param name="expression">Expression to return ID property.</param>
	public static IHtmlContent HiddenForId<TModel, TId>(this IHtmlHelper<TModel> @this, Expression<Func<TModel, TId>> expression)
		where TId : IUnion
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
