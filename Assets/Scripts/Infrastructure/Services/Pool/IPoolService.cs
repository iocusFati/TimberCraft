using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.States;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        Dictionary<ResourceType, DropoutPool> DropoutsPool { get; }
        ParticlePool WoodHitParticlesPool { get; }
        ParticlePool StoneHitParticlesPool { get; }
        ParticlePool LeavesParticlesPool { get; }
        ParticlePool OnLandedParticlesPool { get; }
    }
}