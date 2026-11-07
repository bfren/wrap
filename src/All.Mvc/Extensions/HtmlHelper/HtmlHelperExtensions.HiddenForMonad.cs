// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Linq.Expressions;
using System.Reflection;
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
				GetMonad(expression, model).Value.ToString(),

			_ =>
				null
		};

		// Return hidden input
		return @this.Hidden(name, value ?? string.Empty);
	}

	// read plain `x => x.Property` lambdas by reflection: compiling the expression on every render is slow
	private static TMonad GetMonad<TModel, TMonad>(Expression<Func<TModel, TMonad>> expression, TModel model) =>
		expression.Body switch
		{
			MemberExpression { Member: PropertyInfo property, Expression: ParameterExpression } when property.GetValue(model) is TMonad x =>
				x,

			_ =>
				expression.Compile().Invoke(model)
		};
}
