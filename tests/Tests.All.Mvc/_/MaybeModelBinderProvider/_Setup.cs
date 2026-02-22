// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Wrap.Mvc.MaybeModelBinderProvider_Tests;

public abstract class MaybeModelBinderProvider_Tests
{
	public (MaybeModelBinderProvider, Vars) Setup(Type modelType)
	{
		var identity = ModelMetadataIdentity.ForType(modelType);
		var metadata = Substitute.ForPartsOf<ModelMetadata>(identity);

		var context = Substitute.ForPartsOf<ModelBinderProviderContext>();
		context.Metadata.Returns(metadata);

		return (new(), new(
			context
		));
	}

	public sealed record class Vars(
		ModelBinderProviderContext Context
	);
}
