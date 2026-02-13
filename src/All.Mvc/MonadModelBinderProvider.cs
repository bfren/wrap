// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Exceptions;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="IMonad{TId, TValue}"/> MVC model binder provider.
/// </summary>
public sealed class MonadModelBinderProvider : IModelBinderProvider
{
	/// <inheritdoc/>
	public IModelBinder? GetBinder(ModelBinderProviderContext context)
	{
		// If this type isn't an ID, return null so MVC can move on to try the next model binder
		var (monadType, valueType) = F.GetMonadTypes(context.Metadata.ModelType, typeof(IMonad<,>));
		if (monadType is null || valueType is null)
		{
			return null;
		}

		// Attempt to create and return the binder
		var genericType = typeof(MonadModelBinder<,>).MakeGenericType(monadType, valueType);
		return Activator.CreateInstance(genericType) switch
		{
			IModelBinder x =>
				x,

			_ =>
				throw new ModelBinderException($"Unable to create {typeof(MonadModelBinder<,>)} for type {monadType.Name}.")
		};
	}
}
