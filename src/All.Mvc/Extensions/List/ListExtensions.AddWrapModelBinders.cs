// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc;

public static partial class ListExtensions
{
	/// <summary>
	/// Insert the Wrap model binder providers at the start of the list, so they run before the built-in providers.
	/// </summary>
	/// <remarks>
	/// Adds <see cref="MaybeModelBinderProvider"/> first, then <see cref="MonadModelBinderProvider"/>.
	/// </remarks>
	/// <param name="this">Model binder providers, usually <c>MvcOptions.ModelBinderProviders</c>.</param>
	public static void AddWrapModelBinders(this IList<IModelBinderProvider> @this)
	{
		@this.Insert(0, new MaybeModelBinderProvider());
		@this.Insert(1, new MonadModelBinderProvider());
	}
}
