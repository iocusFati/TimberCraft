using Infrastructure.AssetProviderService;

namespace Infrastructure.Services.Pool
{
    public class PoolService : IPoolService
    {
        private readonly IAssets _assets;
        
        public WoodHitParticlesPool WoodHitParticlesPool { get; private set; }


        public PoolService(IAssets assets)
        {
            _assets = assets;
        }

        public void Initialize()
        {
            InitializeWoodHitParticles();
        }

        private void InitializeWoodHitParticles()
        {
            WoodHitParticlesPool = new WoodHitParticlesPool(_assets);
        }
    }
}