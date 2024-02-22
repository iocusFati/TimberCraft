using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using UnityEngine;

namespace Infrastructure.Services.Pool
{
    public class PoolService : IPoolService
    {
        private readonly IAssets _assets;
        
        public WoodHitParticlesPool WoodHitParticlesPool { get; private set; }
        public BasePool<ParticleSystem> StoneHitParticlesPool { get; private set; }
        public Dictionary<ResourceType, DropoutPool> DropoutsPool { get; private set; }


        public PoolService(IAssets assets)
        {
            _assets = assets;
        }

        public void Initialize()
        {
            InitializeWoodHitParticles();
            InitializeStoneHitParticles();
            InitializeLogsPool();
        }

        private void InitializeWoodHitParticles() => 
            WoodHitParticlesPool = new WoodHitParticlesPool(_assets);
        
        private void InitializeStoneHitParticles() => 
            StoneHitParticlesPool = new StoneHitParticlePool(_assets);
        
        private void InitializeLogsPool() =>
            DropoutsPool = new Dictionary<ResourceType, DropoutPool>
            {
                {ResourceType.Wood, new DropoutPool(_assets, ResourceType.Wood)}, 
                {ResourceType.Stone, new DropoutPool(_assets, ResourceType.Stone)} 
            };
    }
}