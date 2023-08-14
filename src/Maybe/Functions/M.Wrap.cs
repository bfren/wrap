// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public static partial class M
{
	/// <summary>
	/// One of the most important functions in the library: takes a value and returns
	/// either <see cref="Some{T}"/> or <see cref="None"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If <paramref name="value"/> is not null, you will get a <see cref="Some{T}"/> object where
	/// <see cref="Some{T}.Value"/> is <paramref name="value"/>.
	/// </para>
	/// <para>
	/// If <paramref name="value"/> is null and <typeparamref name="T"/> is a nullable value type,
	/// you will get a <see cref="Some{T}"/> object where <see cref="Some{T}.Value"/> is null.
	/// </para>
	/// <para>
	/// If <paramref name="value"/> is null and <typeparamref name="T"/> is a reference type (with or
	/// without the ? suffix), you will get a <see cref="None"/> object.
	/// </para>
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public static Maybe<T> Wrap<T>(T value) =>
		value switch
		{
			T x =>
				new Some<T>(x),

			_ when F.IsNullableValueType(value) =>
				new Some<T>(value!),

			_ =>
				None
		};
}
