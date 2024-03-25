// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	internal sealed record class Failure : Result<T>, ILeft<FailValue, T>
	{
		/// <summary>
		/// Failure value.
		/// </summary>
		public required FailValue Value { get; init; }

		/// <summary>
		/// Creation only via <see cref="Create(FailValue)"/>.
		/// </summary>
		private Failure() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		internal static Result<T> Create(FailValue value) =>
			new Failure() { Value = value };
	}
}
