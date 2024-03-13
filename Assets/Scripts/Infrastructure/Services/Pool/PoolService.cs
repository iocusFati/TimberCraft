using System.Collections.Generic;
using Gameplay.Resource;
using Infrastructure.AssetProviderService;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.PoolData;
using Utils;

namespace Infrastructure.Services.Pool
{
    public class PoolService : IPoolService
    {
        private readonly IAssets _assets;
        private PoolConfig _poolConfig;

        public ParticlePool WoodHitParticlesPool { get; private set; }
        public ParticlePool StoneHitParticlesPool { get; private set; }
        public ParticlePool LeavesParticlesPool { get; private set; }
        public ParticlePool OnLandedParticlesPool { get; private set; }
        public Dictionary<ResourceType, DropoutPool> DropoutsPool { get; private set; }


        public PoolService(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _poolConfig = staticData.PoolConfig;
        }

        public void CreatePools()
        {
            InitializeWoodHitParticles();
            InitializeStoneHitParticles();
            InitializeDropoutsPool();
            InitializeLeavesPool();
            InitializeOnLandedParticles();
        }

        public void Initialize()
        {
            WoodHitParticlesPool.Initialize(_poolConfig.ParticleInitialSpawnNumber);
            StoneHitParticlesPool.Initialize(_poolConfig.ParticleInitialSpawnNumber);
            LeavesParticlesPool.Initialize(_poolConfig.ParticleInitialSpawnNumber);
            OnLandedParticlesPool.Initialize(_poolConfig.ParticleInitialSpawnNumber);
            
            foreach (var pool in DropoutsPool.Values) 
                pool.Initialize(_poolConfig.DropoutInitialSpawnNumber);
        }

        private void InitializeLeavesPool() => 
            LeavesParticlesPool = new ParticlePool(_assets, AssetPaths.LeavesParticle);

        private void InitializeWoodHitParticles() => 
            WoodHitParticlesPool = new ParticlePool(_assets, AssetPaths.WoodHitParticle);

        private void InitializeStoneHitParticles() => 
            StoneHitParticlesPool = new ParticlePool(_assets, AssetPaths.StoneHitParticle);

        private void InitializeOnLandedParticles() => 
            OnLandedParticlesPool = new ParticlePool(_assets, AssetPaths.OnLandedParticle);

        private void InitializeDropoutsPool() =>
            DropoutsPool = new Dictionary<ResourceType, DropoutPool>
            {
                { ResourceType.Wood, new DropoutPool(_assets, ResourceType.Wood) },
                { ResourceType.Stone, new DropoutPool(_assets, ResourceType.Stone) }
            };
    }
}