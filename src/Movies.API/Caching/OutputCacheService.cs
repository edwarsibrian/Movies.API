using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Interfaces;

namespace Movies.API.Caching
{
    public class OutputCacheService : ICacheService
    {
        private readonly IOutputCacheStore _cacheStore;

        public OutputCacheService(IOutputCacheStore cacheStore)
        {
            _cacheStore = cacheStore;
        }

        public Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default)
        {
            return _cacheStore.EvictByTagAsync(tag, cancellationToken).AsTask();
        }
    }
}
