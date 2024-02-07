using System.Linq;
using Infrastructure.Services;
using Unity.VisualScripting;

namespace Infrastructure.States
{
    public interface ICacheService : IService
    {
        CacheContainer<DropoutResource> ResourceDropout { get; }
    }
}