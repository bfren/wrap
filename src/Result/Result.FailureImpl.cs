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
		private readonly F.ExceptionLogger? exceptionLogger;

		private readonly F.FailureLogger? failureLogger;

		/// <summary>
		/// Failure value.
		/// </summary>
		public required FailureValue Value
		{
			get;
			init
			{
				if (exceptionLogger is not null && value.Exception is not null)
				{
					exceptionLogger(value.Exception);
				}
				else if (failureLogger is not null)
				{
					failureLogger(value.Message, value.Args);
				}

				field = value;
			}
		}

		/// <summary>
		/// Internal creation only.
		/// </summary>
		/// <param name="value">FailureValue.</param>
		[SetsRequiredMembers]
		internal FailureImpl(FailureValue value) : this(F.LogException, F.LogFailure, value) { }

		/// <summary>
		/// Internal creation only (for testing).
		/// </summary>
		/// <param name="exceptionLogger">ExceptionLogger.</param>
		/// <param name="failureLogger">FailureLogger.</param>
		/// <param name="value">FailureValue.</param>
		[SetsRequiredMembers]
		internal FailureImpl(F.ExceptionLogger? exceptionLogger, F.FailureLogger? failureLogger, FailureValue value) =>
			(this.exceptionLogger, this.failureLogger, Value) = (exceptionLogger, failureLogger, value);
	}
}
