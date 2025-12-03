using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameGo.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;

namespace GameGo.Infrastructure.Services;

public class CacheService : ICacheService
{
	private readonly IDistributedCache _cache;

	public CacheService(IDistributedCache cache)
	{
		_cache = cache;
	}

	public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
	{
		var cachedData = await _cache.GetStringAsync(key, cancellationToken);

		if (string.IsNullOrEmpty(cachedData))
			return null;

		return JsonSerializer.Deserialize<T>(cachedData);
	}

	public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
	{
		var options = new DistributedCacheEntryOptions();

		if (expiry.HasValue)
		{
			options.AbsoluteExpirationRelativeToNow = expiry.Value;
		}
		else
		{
			options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
		}

		var serializedData = JsonSerializer.Serialize(value);
		await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
	}

	public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
	{
		await _cache.RemoveAsync(key, cancellationToken);
	}

	public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
	{
		var cachedData = await _cache.GetStringAsync(key, cancellationToken);
		return !string.IsNullOrEmpty(cachedData);
	}
}