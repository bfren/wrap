// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;
using Wrap.Exceptions;

namespace Wrap;

/// <summary>
/// Wraps simple CLR types.
/// </summary>
/// <typeparam name="TUnion">Union implementation type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public abstract record class Union<TUnion, TValue> : IUnion<TUnion, TValue>
	where TUnion : IUnion<TUnion, TValue>, new()
{
	/// <inheritdoc cref="IUnion{T}.Value"/>
	[MemberNotNull]
	public TValue Value
	{
		get => Check(value);
		init => this.value = Check(value);
	}

	/// <summary>
	/// Encapsulated value.
	/// </summary>
	private readonly TValue? value;

	/// <summary>
	/// Allow 
	/// </summary>
	protected Union() { }

	/// <summary>
	/// Allow base classes to set .
	/// </summary>
	/// <param name="value">Union value.</param>
	protected Union([DisallowNull] TValue value) =>
		this.value = value;

	/// <summary>
	/// Check <paramref name="value"/> and throw an exception if it is null and the underlying type
	/// does not allow null values.
	/// </summary>
	/// <param name="value">Value to check.</param>
	/// <returns><paramref name="value"/> if not null or null is permitted.</returns>
	/// <exception cref="NullUnionValueException">If <paramref name="value"/> is null and the underlying type is not nullable.</exception>
	private TValue Check(TValue? value) =>
		value switch
		{
			TValue =>
				value,

			_ when F.IsNullableValueType(value) =>
				value!,

			_ =>
				throw new NullUnionValueException()
		};

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
