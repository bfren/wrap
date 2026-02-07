// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Extensions;

namespace Wrap.Mvc;

/// <summary>
/// <see cref="Id{TId, TValue}"/> MVC model binder.
/// </summary>
/// <typeparam name="TId"><see cref="Id{TId, TValue}"/> type.</typeparam>
/// <typeparam name="TIdValue"><see cref="Id{TId, TValue}"/> Value type.</typeparam>
public abstract class IdModelBinder<TId, TIdValue> : IModelBinder
	where TId : Id<TId, TIdValue>, new()
	where TIdValue : struct
{
	/// <summary>
	/// StrongId Value parse function
	/// </summary>
	/// <param name="input">Input string from the model binder</param>
	internal abstract Maybe<TIdValue> Parse(string? input);

	/// <summary>
	/// Default value, used when binding fails
	/// </summary>
	internal abstract TIdValue Default { get; }

	/// <summary>
	/// Get value from the binding context
	/// </summary>
	/// <param name="bindingContext">ModelBindingContext</param>
	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		// Get the value from the context
		var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
		if (valueProviderResult == ValueProviderResult.None)
		{
			return Task.CompletedTask;
		}

		// Set the model state value
		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

		// Get the actual value and attempt to parse it
		bindingContext.Result =
			Parse(
				valueProviderResult.FirstValue
			)
			.Match(
				fSome: x => ModelBindingResult.Success(new TId { Value = x }),
				fNone: () => ModelBindingResult.Success(new TId { Value = Default })
			);

		return Task.CompletedTask;
	}
}
