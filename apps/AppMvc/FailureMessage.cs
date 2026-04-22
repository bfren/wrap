// Wrap: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace AppMvc;

public partial class Test
{
	public partial class Failures
	{

		[Failure("User {userId} was not found", Wrap.Logging.LogLevel.Error)]
		public static partial FailureValue UserNotFound(int userId);
	}
}
