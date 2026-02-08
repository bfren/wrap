// Wrap: Unit Tests.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Text.Json;
using System.Text.Json.Serialization;
using Wrap.Json;

namespace Wrap;

public static partial class Helpers
{
	public static class Json
	{
		public static JsonSerializerOptions Options
		{
			get
			{
				var opt = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
					NumberHandling = JsonNumberHandling.AllowReadingFromString
				};

				opt.Converters.Add(new MaybeJsonConverterFactory());
				opt.Converters.Add(new MonadJsonConverterFactory());
				return opt;
			}
		}

		public static TheoryData<string> Valid_Numeric_Json_Data =>
			[
				"{0}",
				"\"{0}\""
			];

		public static TheoryData<string> Valid_String_Json_Data =>
			[
				"\"{0}\""
			];

		public static TheoryData<string> Invalid_Json_Data =>
			[
				"\"  \"",
				"true",
				"false",
				"[ 0, 1, 2 ]"
			];
	}
}
