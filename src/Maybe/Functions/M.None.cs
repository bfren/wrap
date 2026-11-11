// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Threading.Tasks;

namespace Wrap;

public static partial class M
{
	/// <summary>
	/// Gets a <see cref="Wrap.None"/> value, which converts to any <see cref="Maybe{T}"/>.
	/// </summary>
	public static None None =>
		new();

	/// <summary>
	/// Create a completed task whose result is <see cref="Wrap.None"/>.
	/// </summary>
	/// <typeparam name="T">Some value type.</typeparam>
	/// <returns>Completed task with a <see cref="Wrap.None"/> result.</returns>
	public static Task<Maybe<T>> NoneAsTask<T>() =>
		NoneTask<T>.Value;

	private static class NoneTask<T>
	{
		public static readonly Task<Maybe<T>> Value =
			Task.FromResult(default(Maybe<T>));
	}
}
