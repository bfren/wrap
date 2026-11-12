// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// The 'Right' case of <see cref="Either{TLeft, TRight}"/>, which holds a correct / valid value (see <seealso cref="Left{T}"/>).
/// </summary>
/// <remarks>
/// <see cref="Right{T}"/> does not carry the left value type, so it converts to any <see cref="Either{TLeft, TRight}"/>
/// with a matching right type - you can <c>return new Right&lt;int&gt;(42);</c> without specifying the left type.
/// </remarks>
/// <typeparam name="T">Right value type.</typeparam>
/// <param name="Value">Right value.</param>
public readonly record struct Right<T>(T Value);
