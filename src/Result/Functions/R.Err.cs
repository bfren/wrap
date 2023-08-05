// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Monadic.Exceptions;

namespace Monadic;

public static partial class R
{
	public static Err Err(Exception ex)
	{
		F.LogException(ex);
		return new(ex);
	}

	public static Err Err<TException>()
		where TException : Exception, new() =>
		Err(new TException());

	public static Result<T> Err<T>(Exception ex) =>
		Err(ex);

	public static Err Err(string err)
	{
		F.LogError(err);
		return new(new SimpleErrorException(err));
	}

	public static Result<T> Err<T>(string str) =>
		Err(str);
}
