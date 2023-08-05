// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Monadic;

public abstract partial record class Maybe<T>
{
	internal sealed record class None : Maybe<T>, ILeft<Monadic.None, T>
	{
		public Monadic.None Value { get; private init; }

		internal static Maybe<T> Create() =>
			new None();

		private None() =>
			Value = new();
	}
}
