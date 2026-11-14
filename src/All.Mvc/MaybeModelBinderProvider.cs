// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Exceptions;

namespace Wrap.Mvc;

/// <summary>
/// MVC model binder provider that creates a <see cref="MaybeModelBinder{T}"/> for <see cref="Maybe{T}"/> properties.
/// </summary>
public sealed class MaybeModelBinderProvider : IModelBinderProvider
{
	/// <summary>
	/// Create a model binder if the model type is <see cref="Maybe{T}"/>.
	/// </summary>
	/// <param name="context">Model binder provider context.</param>
	/// <returns>A <see cref="MaybeModelBinder{T}"/>, or <see langword="null"/> if the model type is not <see cref="Maybe{T}"/>, so MVC tries the next provider.</returns>
	/// <exception cref="Exceptions.ModelBinderException">The binder could not be created.</exception>
	public IModelBinder? GetBinder(ModelBinderProviderContext context) =>
		GetBinder(context.Metadata.ModelType);

	internal IModelBinder? GetBinder(Type modelType)
	{
		// If this type isn't Maybe<T>, return null so MVC can move on to try the next model binder
		if (!modelType.IsGenericType || modelType.GetGenericTypeDefinition() != typeof(Maybe<>))
		{
			return null;
		}

		// Build the model binder with the correct value type
		var wrappedType = modelType.GetGenericArguments()[0];
		var binderType = typeof(MaybeModelBinder<>).MakeGenericType(wrappedType);

		// Attempt to create and return the binder
		return Activator.CreateInstance(binderType) switch
		{
			IModelBinder x =>
				x,

			_ =>
				throw new ModelBinderException($"Unable to create {typeof(MaybeModelBinder<>)} for type {modelType.Name}.")
		};
	}
}
