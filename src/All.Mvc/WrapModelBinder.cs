// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Extensions;
using Wrap.Json;

namespace Wrap.Mvc;

internal static class WrapModelBinderHelpers
{
	internal static JsonSerializerOptions Options { get; private set; }

	static WrapModelBinderHelpers()
	{
		Options = new() { NumberHandling = JsonNumberHandling.AllowReadingFromString };
		Options.Converters.Add(new MonadJsonConverterFactory());
	}
}

/// <inheritdoc/>
public abstract class WrapModelBinder<TValue> : IWrapModelBinder<TValue>
{
	private static object? FNone() =>
		null;

	private static object? FSome<TParse>(TParse value) =>
		value;

	/// <summary>
	/// Wrap a value to insert correctly into the binding context.
	/// </summary>
	/// <param name="value">Value to wrap.</param>
	/// <returns>Wrapped value</returns>
	public abstract object Wrap(TValue value);

	/// <summary>
	/// Parse Monad value from the binding context.
	/// </summary>
	/// <param name="bindingContext">ModelBindingContext.</param>
	public virtual Task BindModelAsync(ModelBindingContext bindingContext)
	{
		// Perform bind
		var (valueResult, bindingResult) = GetValue(bindingContext.ValueProvider, bindingContext.ModelName);

		// Set binding values
		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
		bindingContext.Result = bindingResult;

		// No async work to do
		return Task.CompletedTask;
	}

	/// <summary>
	/// Binding abstraction to enable testing.
	/// </summary>
	/// <param name="provider">IValueProvider.</param>
	/// <param name="model">Model Name.</param>
	/// <returns>ValueProviderResult and ModelBindingResult.</returns>
	public (ValueProviderResult valueResult, ModelBindingResult bindingResult) GetValue(
		IValueProvider provider,
		string model
	)
	{
		// Get the value from the context
		var result = provider.GetValue(model);
		if (result == ValueProviderResult.None)
		{
			return Nothing();
		}

		// Ensure value is not null
		var value = result.FirstValue;
		if (string.IsNullOrWhiteSpace(value))
		{
			return Nothing();
		}

		try
		{
			// Attempt to parse the value
			var valueParsed = default(TValue) switch
			{
				bool =>
					M.ParseBool(value).Match(FNone, FSome),

				byte =>
					M.ParseByte(value).Match(FNone, FSome),

				char =>
					M.ParseChar(value).Match(FNone, FSome),

				DateOnly =>
					M.ParseDateOnly(value).Match(FNone, FSome),

				DateTime =>
					M.ParseDateTime(value).Match(FNone, FSome),

				DateTimeOffset =>
					M.ParseDateTimeOffset(value).Match(FNone, FSome),

				decimal =>
					M.ParseDecimal(value).Match(FNone, FSome),

				double =>
					M.ParseDouble(value).Match(FNone, FSome),

				Guid =>
					M.ParseGuid(value).Match(FNone, FSome),

				short =>
					M.ParseInt16(value).Match(FNone, FSome),

				int =>
					M.ParseInt32(value).Match(FNone, FSome),

				long =>
					M.ParseInt64(value).Match(FNone, FSome),

				Int128 =>
					M.ParseInt128(value).Match(FNone, FSome),

				nint =>
					M.ParseIntPtr(value).Match(FNone, FSome),

				float =>
					M.ParseSingle(value).Match(FNone, FSome),

				TimeOnly =>
					M.ParseTimeOnly(value).Match(FNone, FSome),

				ushort =>
					M.ParseUInt16(value).Match(FNone, FSome),

				uint =>
					M.ParseUInt32(value).Match(FNone, FSome),

				ulong =>
					M.ParseUInt64(value).Match(FNone, FSome),

				UInt128 =>
					M.ParseUInt128(value).Match(FNone, FSome),

				nuint =>
					M.ParseUIntPtr(value).Match(FNone, FSome),

				_ =>
					null
			};

			// Wrap parsed value
			return valueParsed switch
			{
				TValue x =>
					(result, ModelBindingResult.Success(Wrap(x))),

				_ when new MonadModelBinderProvider().GetBinder(typeof(TValue)) is IWrapModelBinder<TValue> x =>
					x.GetValue(provider, model),

				_ when JsonSerializer.Deserialize<TValue>($"\"{value}\"", WrapModelBinderHelpers.Options) is TValue x =>
					(result, ModelBindingResult.Success(Wrap(x))),

				_ =>
					Nothing()
			};
		}
		catch
		{
			return Nothing();
		}
	}

	/// <summary>
	/// Return a 'None' or null value.
	/// </summary>
	internal virtual (ValueProviderResult valueResult, ModelBindingResult bindingResult) Nothing() =>
		(ValueProviderResult.None, ModelBindingResult.Failed());
}
