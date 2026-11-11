// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

/// <summary>
/// The 'None' case of <see cref="Maybe{T}"/>, used in place of <see langword="null"/>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="None"/> carries no type, so it converts to any <see cref="Maybe{T}"/> - you can
/// <c>return M.None;</c> without specifying the value type. Use <see cref="M.None"/> to create one.
/// </para>
/// <para>
/// Every <see cref="None"/> is equal to every other <see cref="None"/>.
/// </para>
/// </remarks>
public readonly record struct None;
