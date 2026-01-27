// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wrap.Mvc;

namespace Wrap.Extensions.MvcOptionsExtensions_Tests;

public class InsertProvider_Tests
{
	[Fact]
	public void Inserts_ModelBinderProvider__As_First_Item()
	{
		// Arrange
		var insert = Substitute.For<Action<int, IModelBinderProvider>>();

		// Act
		MvcOptionsExtensions.InsertProvider(insert);

		// Assert
		insert.Received().Invoke(0, Arg.Any<IdModelBinderProvider>());
	}
}
