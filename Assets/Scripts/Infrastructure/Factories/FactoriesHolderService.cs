using Infrastructure.AssetProviderService;
using Infrastructure.States;
using Zenject;

namespace Infrastructure.Factories
{
    public class FactoriesHolderService : IFactoriesHolderService, IInitializable
    {
        private readonly IAssets _assets;
        
        public PlayerFactory PlayerFactory { get; private set; }
        public LocationFactory LocationFactory { get; set; }

        public FactoriesHolderService(IAssets assets)
        {
            _assets = assets;
        }

        public void Initialize()
        {
            InitializeFactories();
        }

        private void InitializeFactories()
        {
            InitializePlayerFactory();
            InitializeLocationFactory();
        }

        private void InitializePlayerFactory() => 
            PlayerFactory = new PlayerFactory(_assets);

        private void InitializeLocationFactory() => 
            LocationFactory = new LocationFactory(_assets);
    }
}