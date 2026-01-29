// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// Create a new <see cref="Wrap.None"/> value.
	/// </summary>
	/// <returns>None value.</returns>
	public static None None =>
		new();

	/// <summary>
	/// Create a new <see cref="Wrap.None"/> value wrapped as a Task.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <returns>None value wrapped as a Task.</returns>
	public static Task<Maybe<T>> NoneAsTask<T>() =>
		new Maybe<T>.NoneImpl().AsTask();
}
