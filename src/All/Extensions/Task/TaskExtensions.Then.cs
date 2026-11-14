// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wrap.Extensions;

internal static partial class TaskExtensions
{
	/// <summary>
	/// Await <paramref name="this"/> and pass the value to <paramref name="f"/>.
	/// </summary>
	/// <typeparam name="TIn">Input value type.</typeparam>
	/// <typeparam name="TOut">Output value type.</typeparam>
	/// <param name="this">Task returning a value.</param>
	/// <param name="f">Function to run on the value.</param>
	/// <returns>The value returned by <paramref name="f"/>.</returns>
	internal static async Task<TOut> Then<TIn, TOut>(this Task<TIn> @this, Func<TIn, TOut> f) =>
		f(await @this);

	/// <inheritdoc cref="Then{TIn, TOut}(Task{TIn}, Func{TIn, TOut})"/>
	internal static async Task<TOut> Then<TIn, TOut>(this Task<TIn> @this, Func<TIn, Task<TOut>> f) =>
		await f(await @this);

	/// <summary>
	/// Await <paramref name="this"/>, pass the list to <paramref name="f"/> and build the result once,
	/// so <paramref name="f"/> is not run again each time the result is read.
	/// </summary>
	/// <typeparam name="TIn">Input list item type.</typeparam>
	/// <typeparam name="TOut">Output list item type.</typeparam>
	/// <param name="this">Task returning a list.</param>
	/// <param name="f">Function to run on the list.</param>
	/// <returns>The items returned by <paramref name="f"/>, read into a new collection.</returns>
	internal static async Task<IEnumerable<TOut>> Then<TIn, TOut>(this Task<IEnumerable<TIn>> @this, Func<IEnumerable<TIn>, IEnumerable<TOut>> f) =>
		[.. f(await @this)];

	/// <summary>
	/// Await <paramref name="this"/> and pass the list to <paramref name="f"/>.
	/// </summary>
	/// <typeparam name="T">List item type.</typeparam>
	/// <param name="this">Task returning a list.</param>
	/// <param name="f">Action to run on the list.</param>
	/// <returns>A task that completes when <paramref name="f"/> has run.</returns>
	internal static async Task Then<T>(this Task<IEnumerable<T>> @this, Action<IEnumerable<T>> f) =>
		f(await @this);

	/// <inheritdoc cref="Then{T}(Task{IEnumerable{T}}, Action{IEnumerable{T}})"/>
	internal static async Task Then<T>(this Task<IEnumerable<T>> @this, Func<IEnumerable<T>, Task> f) =>
		await f(await @this);
}
