// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc;
using Wrap.Mvc;

namespace Wrap.Extensions;

public static partial class MvcOptionsExtensions
{
	/// <summary>
	/// Insert <see cref="IdModelBinderProvider"/> into the MVC options
	/// </summary>
	/// <param name="this"></param>
	public static void AddIdModelBinder(this MvcOptions @this) =>
		InsertProvider(@this.ModelBinderProviders.Insert);
}
