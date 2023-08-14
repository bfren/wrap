// Monadic: .NET monads for functional style.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monadic;

public static partial class MaybeExtensions
{
	public static TSingle UnwrapSingle<T, TSingle>(this Maybe<T> @this, TSingle ifNone)
		where T : IEnumerable<TSingle> =>
		Switch(@this,
			none: ifNone,
			some: x => x.Single()
		);

	public static TSingle UnwrapSingle<T, TSingle>(this Maybe<T> @this, Func<TSingle> ifNone)
		where T : IEnumerable<TSingle> =>
		Switch(@this,
			none: ifNone,
			some: x => x.Single()
		);

	public static Task<TSingle> UnwrapSingleAsync<T, TSingle>(this Maybe<T> @this, Func<Task<TSingle>> ifNone)
		where T : IEnumerable<TSingle> =>
		SwitchAsync(@this,
			none: ifNone,
			some: x => x.Single()
		);

	public static Task<TSingle> UnwrapSingleAsync<T, TSingle>(this Task<Maybe<T>> @this, TSingle ifNone)
		where T : IEnumerable<TSingle> =>
		SwitchAsync(@this,
			none: ifNone,
			some: x => x.Single()
		);

	public static Task<TSingle> UnwrapSingleAsync<T, TSingle>(this Task<Maybe<T>> @this, Func<TSingle> ifNone)
		where T : IEnumerable<TSingle> =>
		SwitchAsync(@this,
			none: ifNone,
			some: x => x.Single()
		);

	public static Task<TSingle> UnwrapSingleAsync<T, TSingle>(this Task<Maybe<T>> @this, Func<Task<TSingle>> ifNone)
		where T : IEnumerable<TSingle> =>
		SwitchAsync(@this,
			none: ifNone,
			some: x => x.Single()
		);
}
