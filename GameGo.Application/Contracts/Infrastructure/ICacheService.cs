using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameGo.Application.Contracts.Infrastructure;

public interface ICacheService
{
	Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;
	Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class;
	Task RemoveAsync(string key, CancellationToken cancellationToken = default);
	Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}