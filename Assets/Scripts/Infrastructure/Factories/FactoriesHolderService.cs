using Infrastructure.AssetProviderService;
using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Factories.PlayerFactoryFolder;
using Zenject;

namespace Infrastructure.Factories
{
    public class FactoriesHolderService : IFactoriesHolderService, IInitializable
    {
        private readonly IAssets _assets;
        private readonly IInstantiator _instantiator;

        public PlayerFactory PlayerFactory { get; private set; }
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
            InitializeBotFactory();
        }

        private void InitializePlayerFactory() => 
            PlayerFactory = new PlayerFactory(_instantiator);
        
        private void InitializeBotFactory() => 
            BotFactory = new BotFactory(_instantiator);
    }
}