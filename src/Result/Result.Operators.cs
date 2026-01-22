// Wrap: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System.Linq;

namespace Wrap;

public abstract partial record class Result<T>
{
	/// <summary>
	/// Implicitly wrap <paramref name="value"/> as an <see cref="Ok{T}"/> object.
	/// </summary>
	/// <param name="value">Input value.</param>
	/// <returns>Wrapped value.</returns>
	public static implicit operator Result<T>(T value) =>
		R.Wrap(value);

	/// <summary>
	/// Implicitly convert a <see cref="Fail"/> to a <see cref="Result{T}"/> failure.
	/// </summary>
	/// <param name="fail">Fail object.</param>
	/// <returns>Failure value.</returns>
	public static implicit operator Result<T>(Fail fail) =>
		Failure.Create(fail.Value);

	/// <summary>
	/// Implicitly convert a <see cref="FluentFailure"/> to a <see cref="Result{T}"/> failure.
	/// </summary>
	/// <param name="fluent">FluentFail object.</param>
	/// <returns>Failure value.</returns>
	public static implicit operator Result<T>(FluentFailure fluent)
	{
		if (F.LogException is not null && fluent.Value.Exception is not null)
		{
			F.LogException?.Invoke(fluent.Value.Exception);
		}
		else if (F.LogFailure is not null)
		{
			var args = (fluent.Value.Args ?? []).Select(x => x != null);
			F.LogFailure?.Invoke(fluent.Value.Message, [.. args]);
		}

		return Failure.Create(fluent.Value);
	}
}
