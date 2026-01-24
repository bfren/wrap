// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Mvc.ModelBinding;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="MvcOptions"/> extension methods
/// </summary>
public static class MvcOptionsExtensions
{
	/// <summary>
	/// Insert <see cref="IdModelBinderProvider"/> into the MVC options
	/// </summary>
	/// <param name="this"></param>
	public static void AddStrongIdModelBinder(this MvcOptions @this) =>
		InsertProvider(@this.ModelBinderProviders.Insert);

	/// <summary>
	/// Abstract inserting provider to enable testing
	/// </summary>
	/// <param name="insert"></param>
	internal static void InsertProvider(Action<int, IModelBinderProvider> insert) =>
		insert(0, new IdModelBinderProvider());
}
