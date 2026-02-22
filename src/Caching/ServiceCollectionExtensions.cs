// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.DependencyInjection;

namespace Wrap.Caching;

/// <summary>
/// <see cref="ServiceCollection"/> extension methods
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add a <see cref="IWrapCache{TKey}"/> to <paramref name="this"/> with a Singleton lifetime.
	/// </summary>
	/// <typeparam name="TKey">Cache Key type.</typeparam>
	/// <param name="this">IServiceCollection.</param>
	public static IServiceCollection AddWrapCache<TKey>(this IServiceCollection @this)
		where TKey : notnull =>
		@this.AddSingleton<IWrapCache<TKey>, WrapCache<TKey>>();
}
