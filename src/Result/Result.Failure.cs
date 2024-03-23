// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Wrap;

public abstract partial record class Result<T>
{
	internal sealed record class Failure : Result<T>, ILeft<FailValue, T>
	{
		/// <summary>
		/// Failure value.
		/// </summary>
		public FailValue Value { get; init; }

		internal static Result<T> Create(string message) =>
			new Failure(message);

		internal static Result<T> Create(Exception exception) =>
			new Failure(exception);

		internal static Result<T> Create(FailValue value) =>
			new Failure(value);

		/// <summary>
		/// Internal creation only.
		/// </summary>
		/// <param name="value">Failure value.</param>
		private Failure(FailValue value) =>
			Value = value;
	}
}
