// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Mvc;

namespace Wrap.Extensions;

public static partial class MvcOptionsExtensions
{
	/// <summary>
	/// Abstract inserting provider to enable testing
	/// </summary>
	/// <param name="insert"></param>
	internal static void InsertProvider(Action<int, IModelBinderProvider> insert) =>
		insert(0, new IdModelBinderProvider());
}
