using Gameplay.Bots.StateMachine.States;
using Gameplay.Buildings;
using Gameplay.Resource;

namespace Infrastructure.Services.Cache
{
    public interface ICacheService : IService
    {
        CacheContainer<DropoutResource> ResourceDropout { get; }
        CacheContainer<Building> Buildings { get; }
    }
}