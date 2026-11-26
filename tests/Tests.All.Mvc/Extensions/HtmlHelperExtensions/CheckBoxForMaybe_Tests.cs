// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;

namespace Wrap.Mvc.HtmlHelperExtensions_Tests;

public class CheckBoxForMaybe_Tests
{
	private static (IHtmlHelper<TestModel>, string) Setup(TestModel? model)
	{
		var metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(TestModel)));
		var provider = Substitute.For<IModelMetadataProvider>();
		provider.GetMetadataForType(typeof(TestModel)).Returns(metadata);

		var viewData = new ViewDataDictionary<TestModel>(provider, new ModelStateDictionary()) { Model = model! };

		var htmlHelper = Substitute.For<IHtmlHelper<TestModel>>();
		htmlHelper.ViewData
			.Returns(viewData);

		var name = Rnd.Str;
		htmlHelper.NameFor(Arg.Any<Expression<Func<TestModel, Maybe<bool>>>>())
			.Returns(name);

		return (htmlHelper, name);
	}

	public static TheoryData<TestModel?, bool> Checked_Data =>
		new()
		{
			{ new() { Agree = true }, true },
			{ new() { Agree = false }, false },
			{ new() { Agree = M.None }, false },
			{ null, false }
		};

	[Theory]
	[MemberData(nameof(Checked_Data))]
	public void Calls_CheckBox__Checked_Only_For_Some_True(TestModel? model, bool expected)
	{
		// Arrange
		var (htmlHelper, name) = Setup(model);

		// Act
		_ = htmlHelper.CheckBoxForMaybe(x => x.Agree);

		// Assert
		htmlHelper.Received().CheckBox(name, expected, null);
	}

	[Fact]
	public void Passes_HtmlAttributes_To_CheckBox()
	{
		// Arrange
		var (htmlHelper, name) = Setup(new() { Agree = true });
		var attributes = new { @class = Rnd.Str };

		// Act
		_ = htmlHelper.CheckBoxForMaybe(x => x.Agree, attributes);

		// Assert
		htmlHelper.Received().CheckBox(name, true, attributes);
	}

	[Theory]
	[InlineData(new[] { "true", "false" }, true)] // ticked: the checkbox value comes before the hidden "false"
	[InlineData(new[] { "false" }, false)] // not ticked: only the hidden input is sent
	public void Posted_Values__Bind_As_Some(string[] posted, bool expected)
	{
		// Arrange
		var modelName = Rnd.Str;
		var provider = Substitute.For<IValueProvider>();
		provider.GetValue(modelName).Returns(new ValueProviderResult(new StringValues(posted)));
		var binder = new MaybeModelBinder<bool>();

		// Act
		var (_, bindingResult) = binder.GetValue(provider, modelName);

		// Assert
		Assert.True(bindingResult.IsModelSet);
		Assert.IsType<Maybe<bool>>(bindingResult.Model, false).AssertSome(expected);
	}

	public sealed record class TestModel
	{
		public Maybe<bool> Agree { get; init; }
	}
}
