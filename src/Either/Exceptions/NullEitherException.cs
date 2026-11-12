// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap.Exceptions;

/// <summary>
/// Thrown by <see cref="E.Match{TEither, TLeft, TRight, TReturn}(TEither, System.Func{TLeft, TReturn}, System.Func{TRight, TReturn})"/>
/// when the either object is <see langword="null"/>.
/// </summary>
/// <remarks>
/// <see cref="Either{TLeft, TRight}"/>, <c>Maybe&lt;T&gt;</c> and <c>Result&lt;T&gt;</c> are structs and can never be
/// <see langword="null"/>, so this only happens for a reference type that implements <see cref="IEither{TEither, TLeft, TRight}"/>.
/// </remarks>
public sealed class NullEitherException : WrapException { }
