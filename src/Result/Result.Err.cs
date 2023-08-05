// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;

namespace Monadic;

public abstract partial record class Result<T>
{
	internal sealed record class Err : Result<T>, ILeft<Exception, T>
	{
		public Exception Value { get; private init; }

		internal static Result<T> Create(Exception ex) =>
			new Err(ex);

		/// <summary>
		/// Internal creation only.
		/// </summary>
		/// <param name="ex">Exception value.</param>
		private Err(Exception ex) =>
			Value = ex;
	}
}
