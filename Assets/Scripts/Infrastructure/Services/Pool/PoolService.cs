using Infrastructure.AssetProviderService;
using Infrastructure.States;

namespace Infrastructure.Services.Pool
{
    public class PoolService : IPoolService
    {
        private readonly IAssets _assets;
        
        public WoodHitParticlesPool WoodHitParticlesPool { get; private set; }
        public BasePool<DropoutResource> LogsPool { get; private set; }


        public PoolService(IAssets assets)
        {
            _assets = assets;
        }

        public void Initialize()
        {
            InitializeWoodHitParticles();
            InitializeLogsPool();
        }

        private void InitializeWoodHitParticles() => 
            WoodHitParticlesPool = new WoodHitParticlesPool(_assets);
        
        private void InitializeLogsPool() => 
            LogsPool = new LogsPool(_assets);
    }
}