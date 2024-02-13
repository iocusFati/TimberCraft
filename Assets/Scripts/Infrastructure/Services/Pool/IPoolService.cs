using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.States;

namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        WoodHitParticlesPool WoodHitParticlesPool { get; }
        Dictionary<ResourceType, DropoutPool> DropoutsPool { get; }
    }
}