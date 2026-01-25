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
		public required FailValue Value
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
		/// Creation only via <see cref="Create(FailValue)"/>.
		/// </summary>
		private Failure() { }

		/// <summary>
		/// Create a failure result from a pre-existing <see cref="FailValue"/>.
		/// </summary>
		/// <param name="value">FailValue.</param>
		/// <returns>Failure result.</returns>
		internal static Result<T> Create(FailValue value) =>
			new Failure() { Value = value };
	}
}
