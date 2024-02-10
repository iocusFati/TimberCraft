using Gameplay.Resource;

namespace Infrastructure.Services.Cache
{
    public interface ICacheService : IService
    {
        CacheContainer<DropoutResource> ResourceDropout { get; }
    }
}