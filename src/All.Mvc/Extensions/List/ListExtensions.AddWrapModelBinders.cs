// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

public static partial class ListExtensions
{
	/// <summary>
	/// Insert Wrap Model Binders.
	/// </summary>
	/// <param name="this">IModelBinderProvider list.</param>
	public static void AddWrapModelBinders(this IList<IModelBinderProvider> @this)
	{
		@this.Insert(0, new MaybeModelBinderProvider());
		@this.Insert(1, new MonadModelBinderProvider());
	}
}
