// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wrap;

/// <summary>
/// Array that compares by content, so generator models that hold one stay equal between runs and the
/// incremental cache can skip regenerating them (a plain array compares by reference).
/// </summary>
/// <typeparam name="T">Item type.</typeparam>
/// <param name="items">Items.</param>
internal readonly struct EquatableArray<T>(T[] items) : IEnumerable<T>, System.IEquatable<EquatableArray<T>>
{
	// default(EquatableArray<T>) has no array, so treat it as empty
	private T[] Items { get => field ?? []; } =
		items;

	/// <summary>
	/// Gets the number of items.
	/// </summary>
	internal int Length =>
		Items.Length;

	/// <summary>
	/// Gets the item at <paramref name="index"/>.
	/// </summary>
	/// <param name="index">Item index.</param>
	internal T this[int index] =>
		Items[index];

	/// <inheritdoc/>
	public bool Equals(EquatableArray<T> other) =>
		Items.SequenceEqual(other.Items);

	/// <inheritdoc/>
	public override bool Equals(object? obj) =>
		obj is EquatableArray<T> other && Equals(other);

	/// <inheritdoc/>
	public override int GetHashCode() =>
		Items.Aggregate(17, (hash, item) => unchecked((hash * 31) + (item?.GetHashCode() ?? 0)));

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() =>
		((IEnumerable<T>)Items).GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

	/// <summary>
	/// Wrap an array.
	/// </summary>
	/// <param name="items">Items.</param>
	public static implicit operator EquatableArray<T>(T[] items) =>
		new(items);
}
