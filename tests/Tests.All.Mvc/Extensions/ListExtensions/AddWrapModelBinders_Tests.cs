// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wrap.Mvc.ListExtensions_Tests;

public class AddWrapModelBinders_Tests
{
	[Fact]
	public void Inserts_MonadModelBinderProvider()
	{
		// Arrange
		var list = Substitute.For<IList<IModelBinderProvider>>();

		// Act
		list.AddWrapModelBinders();

		// Assert
		list.Received().Insert(0, Arg.Any<MaybeModelBinderProvider>());
		list.Received().Insert(1, Arg.Any<MonadModelBinderProvider>());
	}
}
