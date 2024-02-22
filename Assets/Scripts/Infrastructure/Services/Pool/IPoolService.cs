using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        Dictionary<ResourceType, DropoutPool> DropoutsPool { get; }
        WoodHitParticlesPool WoodHitParticlesPool { get; }
        BasePool<ParticleSystem> StoneHitParticlesPool { get; }
    }
}