// Wrap: Functional Monads for .NET
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2019

using Microsoft.Extensions.DependencyInjection;

namespace Wrap.Caching;

/// <summary>
/// Extension methods to register Wrap caching services with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Register <see cref="WrapCache{TKey}"/> as the <see cref="IWrapCache{TKey}"/> service, with a singleton lifetime.
	/// </summary>
	/// <remarks>
	/// The cache stores its entries in <see cref="Microsoft.Extensions.Caching.Memory.IMemoryCache"/>, which this method
	/// does not register: call <c>AddMemoryCache()</c> as well.
	/// </remarks>
	/// <typeparam name="TKey">Cache key type.</typeparam>
	/// <param name="this">Service collection.</param>
	/// <returns><paramref name="this"/>, so calls can be chained.</returns>
	public static IServiceCollection AddWrapCache<TKey>(this IServiceCollection @this)
		where TKey : notnull =>
		@this.AddSingleton<IWrapCache<TKey>, WrapCache<TKey>>();
}
