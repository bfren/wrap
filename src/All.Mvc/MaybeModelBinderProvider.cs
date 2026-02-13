// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Exceptions;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Maybe{T}"/> MVC model binder provider.
/// </summary>
public sealed class MaybeModelBinderProvider : IModelBinderProvider
{
	/// <inheritdoc/>
	public IModelBinder? GetBinder(ModelBinderProviderContext context)
	{
		// If this type isn't Maybe<T>, return null so MVC can move on to try the next model binder
		var type = context.Metadata.ModelType;
		if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Maybe<>))
		{
			return null;
		}

		// Build the model binder with the correct value type
		var wrappedType = type.GetGenericArguments()[0];
		var binderType = typeof(MaybeModelBinder<>).MakeGenericType(wrappedType);

		// Attempt to create and return the binder
		return Activator.CreateInstance(binderType) switch
		{
			IModelBinder x =>
				x,

			_ =>
				throw new ModelBinderException($"Unable to create {typeof(MaybeModelBinder<>)} for type {type.Name}.")
		};
	}
}
