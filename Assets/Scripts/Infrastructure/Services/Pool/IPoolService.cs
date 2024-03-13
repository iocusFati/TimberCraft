using System.Collections.Generic;
using Gameplay.Resource;

namespace Infrastructure.Services.Pool
{
    public interface IPoolService : IService
    {
        Dictionary<ResourceType, DropoutPool> DropoutsPool { get; }
        ParticlePool WoodHitParticlesPool { get; }
        ParticlePool StoneHitParticlesPool { get; }
        ParticlePool LeavesParticlesPool { get; }
        ParticlePool OnLandedParticlesPool { get; }
        void Initialize();
    }
}