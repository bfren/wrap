// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

// parameters are used by the generated implementation, which dotnet format cannot see before the generator is built
#pragma warning disable IDE0060

namespace AppMvc;

public partial class Test
{
	public partial class Failures
	{

		[Failure("User {userId} was not found", Wrap.Logging.LogLevel.Error)]
		public static partial Failure UserNotFound(int userId);
	}
}
