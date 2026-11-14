// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Exceptions;

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder provider that creates a <see cref="MonadModelBinder{TMonad, TValue}"/> for
/// <see cref="IMonad{TMonad, TValue}"/> properties, such as strongly typed IDs.
/// </summary>
public sealed class MonadModelBinderProvider : IModelBinderProvider
{
	/// <summary>
	/// Create a model binder if the model type implements <see cref="IMonad{TMonad, TValue}"/> once.
	/// </summary>
	/// <param name="context">Model binder provider context.</param>
	/// <returns>A <see cref="MonadModelBinder{TMonad, TValue}"/>, or <see langword="null"/> if the model type is not a single-case monad, so MVC tries the next provider.</returns>
	/// <exception cref="Exceptions.ModelBinderException">The binder could not be created.</exception>
	public IModelBinder? GetBinder(ModelBinderProviderContext context) =>
		GetBinder(context.Metadata.ModelType);

	internal IModelBinder? GetBinder(Type modelType)
	{
		// If this type isn't an ID, return null so MVC can move on to try the next model binder
		var (monadType, valueType) = F.GetMonadTypes(modelType, typeof(IMonad<,>));
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
