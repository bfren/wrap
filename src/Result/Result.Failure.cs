// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Internal implementation of <see cref="Result{T}"/> to 
	/// </summary>
	internal sealed record class Failure : Result<T>, ILeft<FailureValue, T>
	{
		/// <summary>
		/// Failure value.
		/// </summary>
		public required FailureValue Value
		{
			get;
			init
			{
				if (F.LogException is not null && value.Exception is not null)
				{
					F.LogException?.Invoke(value.Exception);
				}
				else if (F.LogFailure is not null)
				{
					F.LogFailure?.Invoke(value.Message, value.Args);
				}

				field = value;
			}
		}

		/// <summary>
		/// Creation only via <see cref="Create(FailureValue)"/>.
		/// </summary>
		private Failure() { }

		/// <summary>
		/// Create a failure result from a pre-existing <see cref="FailureValue"/>.
		/// </summary>
		/// <param name="value">FailValue.</param>
		/// <returns>Failure result.</returns>
		internal static Result<T> Create(FailureValue value) =>
			new Failure() { Value = value };
	}
}
