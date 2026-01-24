// Wrap: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wraps.Id_Tests;

public abstract class Value_Tests<TId, TValue>
	where TId : IId<TId, TValue>, new()
	where TValue : struct
{
}
