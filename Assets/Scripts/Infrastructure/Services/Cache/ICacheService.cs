using Gameplay.Bots.StateMachine.States;
using Gameplay.Buildings;
using Gameplay.Player.ObstacleFade;
using Gameplay.Resource;
using UnityEngine;

namespace Infrastructure.Services.Cache
{
    public interface ICacheService : IService
    {
        CacheContainer<DropoutResource> ResourceDropout { get; }
        CacheContainer<Building> Buildings { get; }
        CacheContainer<ResourceSource> ResourceSources { get; }
        CacheContainer<IObscurablePlayer> ObscureViewObjects { get; }
    }
}