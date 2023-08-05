// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using Monadic.Exceptions;

namespace Monadic;

public static partial class R
{
	public static Err Err(Exception ex)
	{
		LogException(ex);
		return new(ex);
	}

	public static Err Err<TException>()
		where TException : Exception, new() =>
		Err(new TException());

	public static Err Err(string err)
	{
		LogError(err);
		return new(new SimpleErrorException(err));
	}

	//public static Task<Err> ErrAsync(string err) =>
	//	Err(err).AsTask();

	//public static Task<Err> ErrAsync(Exception ex) =>
	//	Err(ex).AsTask();

	//public static Task<Err> ErrAsync<TException>()
	//	where TException : Exception, new() =>
	//	ErrAsync(new TException());
}
