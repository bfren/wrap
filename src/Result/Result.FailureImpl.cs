// Wrap: .NET monads.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Diagnostics.CodeAnalysis;

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Internal implementation of <see cref="Result{T}"/> to 
	/// </summary>
	internal sealed record class FailureImpl : Result<T>, ILeft<FailureValue, T>
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
		/// Internal creation only.
		/// </summary>
		/// <param name="value">FailureValue.</param>
		[SetsRequiredMembers]
		internal FailureImpl(FailureValue value) =>
			Value = value;
	}
}
