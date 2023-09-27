// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public abstract partial record class Result<T>
{
	internal sealed record class Err : Result<T>, ILeft<ErrValue, T>
	{
		public ErrValue Value { get; init; }

		internal static Result<T> Create(string message) =>
			new Err(message);

		internal static Result<T> Create(Exception exception) =>
			new Err(exception);

		internal static Result<T> Create(ErrValue value) =>
			new Err(value);

		/// <summary>
		/// Internal creation only.
		/// </summary>
		/// <param name="value">Error value.</param>
		private Err(ErrValue value) =>
			Value = value;
	}
}
