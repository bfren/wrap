// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Wrap.Mvc.HtmlHelperExtensions_Tests;

public class TextBoxForMaybe_Tests
{
	private static (IHtmlHelper<TestModel>, ViewDataDictionary<TestModel>, string) Setup(TestModel? model)
	{
		var metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(TestModel)));
		var provider = Substitute.For<IModelMetadataProvider>();
		provider.GetMetadataForType(typeof(TestModel)).Returns(metadata);

		var viewData = new ViewDataDictionary<TestModel>(provider, new ModelStateDictionary()) { Model = model! };

		var htmlHelper = Substitute.For<IHtmlHelper<TestModel>>();
		htmlHelper.ViewData
			.Returns(viewData);

		var name = Rnd.Str;
		htmlHelper.NameFor(Arg.Any<Expression<Func<TestModel, Maybe<decimal>>>>())
			.Returns(name);

		return (htmlHelper, viewData, name);
	}

	[Fact]
	public void Some__Calls_TextBox__With_Value_In_DefaultCulture()
	{
		// Arrange
		var (htmlHelper, _, name) = Setup(new() { Amount = 1.5m });

		// Act
		_ = htmlHelper.TextBoxForMaybe(x => x.Amount);

		// Assert
		htmlHelper.Received().TextBox(name, 1.5m.ToString(F.DefaultCulture), null, null);
	}

	[Fact]
	public void None__Calls_TextBox__With_Empty_String()
	{
		// Arrange
		var (htmlHelper, _, name) = Setup(new() { Amount = M.None });

		// Act
		_ = htmlHelper.TextBoxForMaybe(x => x.Amount);

		// Assert
		htmlHelper.Received().TextBox(name, string.Empty, null, null);
	}

	[Fact]
	public void Model_Is_Null__Calls_TextBox__With_Empty_String()
	{
		// Arrange
		var (htmlHelper, _, name) = Setup(null);

		// Act
		_ = htmlHelper.TextBoxForMaybe(x => x.Amount);

		// Assert
		htmlHelper.Received().TextBox(name, string.Empty, null, null);
	}

	[Fact]
	public void Passes_HtmlAttributes_To_TextBox()
	{
		// Arrange
		var (htmlHelper, _, name) = Setup(new() { Amount = 42m });
		var attributes = new { @class = Rnd.Str };

		// Act
		_ = htmlHelper.TextBoxForMaybe(x => x.Amount, attributes);

		// Assert
		htmlHelper.Received().TextBox(name, 42m.ToString(F.DefaultCulture), null, attributes);
	}

	[Fact]
	public void Some_Monad__Calls_TextBox__With_Inner_Value()
	{
		// Arrange
		var value = Rnd.Str;
		var model = new PostcodeModel { Postcode = new Postcode { Value = value } };
		var metadata = Substitute.ForPartsOf<ModelMetadata>(ModelMetadataIdentity.ForType(typeof(PostcodeModel)));
		var provider = Substitute.For<IModelMetadataProvider>();
		provider.GetMetadataForType(typeof(PostcodeModel)).Returns(metadata);
		var viewData = new ViewDataDictionary<PostcodeModel>(provider, new ModelStateDictionary()) { Model = model };
		var htmlHelper = Substitute.For<IHtmlHelper<PostcodeModel>>();
		htmlHelper.ViewData.Returns(viewData);
		var name = Rnd.Str;
		htmlHelper.NameFor(Arg.Any<Expression<Func<PostcodeModel, Maybe<Postcode>>>>()).Returns(name);

		// Act
		_ = htmlHelper.TextBoxForMaybe(x => x.Postcode);

		// Assert
		htmlHelper.Received().TextBox(name, value, null, null);
	}

	public sealed record class TestModel
	{
		public Maybe<decimal> Amount { get; init; }
	}

	public sealed record class PostcodeModel
	{
		public Maybe<Postcode> Postcode { get; init; }
	}

	public sealed record class Postcode : Monad<Postcode, string>;
}
