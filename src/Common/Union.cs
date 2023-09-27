// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;
using Wrap.Exceptions;

namespace Wrap;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TUnion"></typeparam>
/// <typeparam name="TValue"></typeparam>
public abstract record class Union<TUnion, TValue> : IUnion<TValue>
	where TUnion : Union<TUnion, TValue>, new()
{
	/// <inheritdoc cref="IUnion{T}.Value"/>
	[MemberNotNull]
	public TValue Value
	{
		get => value switch
		{
			TValue =>
				value,

			_ when F.IsNullableValueType(value) =>
				value!,

			_ =>
				throw new NullUnionValueException()
		};
		init => this.value = value;
	}

	private readonly TValue? value;

	/// <summary>
	/// 
	/// </summary>
	protected Union() { }

	/// <summary>
	/// Create Union with a value.
	/// </summary>
	/// <param name="value">Union value.</param>
	protected Union([DisallowNull] TValue value) =>
		this.value = value;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
#pragma warning disable CA1000 // Do not declare static members on generic types
	public static TUnion Wrap([DisallowNull] TValue value) =>
		F.Wrap<TUnion, TValue>(value);
#pragma warning restore CA1000 // Do not declare static members on generic types
}

internal class Foo
{
	private void Bar()
	{

	}
}

