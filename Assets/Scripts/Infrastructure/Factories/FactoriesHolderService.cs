using Infrastructure.AssetProviderService;
using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Factories.Location;
using Infrastructure.Factories.PlayerFactoryFolder;
using Infrastructure.States;
using Zenject;

namespace Infrastructure.Factories
{
    public class FactoriesHolderService : IFactoriesHolderService, IInitializable
    {
        private readonly IAssets _assets;
        private readonly IInstantiator _instantiator;

        public PlayerFactory PlayerFactory { get; private set; }
        public LocationFactory LocationFactory { get; private set; }
        public BotFactory BotFactory { get; set; }

        public FactoriesHolderService(IAssets assets, IInstantiator instantiator)
        {
            _assets = assets;
            _instantiator = instantiator;
        }

        public void Initialize()
        {
            InitializeFactories();
        }

        private void InitializeFactories()
        {
            InitializePlayerFactory();
            InitializeLocationFactory();
            InitializeBotFactory();
        }

        private void InitializePlayerFactory() => 
            PlayerFactory = new PlayerFactory(_instantiator);

        private void InitializeLocationFactory() => 
            LocationFactory = new LocationFactory(_assets);
        
        private void InitializeBotFactory() => 
            BotFactory = new BotFactory(_instantiator);
    }
}