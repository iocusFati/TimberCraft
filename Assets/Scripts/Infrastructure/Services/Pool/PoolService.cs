using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using UnityEngine;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class PoolService : IPoolService
    {
        private readonly IAssets _assets;
        
        public ParticlePool WoodHitParticlesPool { get; private set; }
        public ParticlePool StoneHitParticlesPool { get; private set; }
        public ParticlePool LeavesParticlesPool { get; set; }
        public ParticlePool OnLandedParticlesPool { get; set; }
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
            InitializeLeavesPool();
            InitializeOnLandedParticles();
        }

        private void InitializeLeavesPool() => 
            LeavesParticlesPool = new ParticlePool(_assets, AssetPaths.LeavesParticle);

        private void InitializeWoodHitParticles() => 
            WoodHitParticlesPool = new ParticlePool(_assets, AssetPaths.WoodHitParticle);
        
        private void InitializeStoneHitParticles() => 
            StoneHitParticlesPool = new ParticlePool(_assets, AssetPaths.StoneHitParticle);
        
        private void InitializeOnLandedParticles() => 
            OnLandedParticlesPool = new ParticlePool(_assets, AssetPaths.OnLandedParticle);
        
        private void InitializeLogsPool() =>
            DropoutsPool = new Dictionary<ResourceType, DropoutPool>
            {
                {ResourceType.Wood, new DropoutPool(_assets, ResourceType.Wood)}, 
                {ResourceType.Stone, new DropoutPool(_assets, ResourceType.Stone)} 
            };
    }
}