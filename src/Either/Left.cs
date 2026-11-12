// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// The 'Left' case of <see cref="Either{TLeft, TRight}"/>, which holds an error / invalid value (see <seealso cref="Right{T}"/>).
/// </summary>
/// <remarks>
/// <see cref="Left{T}"/> does not carry the right value type, so it converts to any <see cref="Either{TLeft, TRight}"/>
/// with a matching left type - you can <c>return new Left&lt;string&gt;("…");</c> without specifying the right type.
/// </remarks>
/// <typeparam name="T">Left value type.</typeparam>
/// <param name="Value">Left value.</param>
public readonly record struct Left<T>(T Value) : IMonad<T>;
