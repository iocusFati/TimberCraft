using System.Threading.Tasks;
using Gameplay.Buildings;
using Gameplay.Player;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.Factories;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.ResourcesData;

namespace Gameplay.Resource
{
    public class ResourcesSelling : IResourcesSelling
    {
        private readonly IGameResourceStorage _gameResourceStorage;
        private readonly ResourcesConfig _resourcesConfig;

        private readonly int _resourceUnitsPerCoin;
        private PlayerResourceShare _playerSharing;

        public ResourcesSelling(IGameResourceStorage gameResourceStorage, IStaticDataService staticData,
            IFactoriesHolderService factories)
        {
            _gameResourceStorage = gameResourceStorage;
            _resourcesConfig = staticData.ResourcesConfig;

            factories.PlayerFactory.OnPlayerCreated += player => _playerSharing = player.ResourceShare;
        }
        
        public int GetMaxSellResourceCount(ResourceType type)
        {
            int resourceUnitsPerCoin = GetResourceUnitsPerCoin(type);
            
            int receiveCoins = _gameResourceStorage.GetResourceCountOfType(type) / resourceUnitsPerCoin;
            int sellResourcesCount = receiveCoins * resourceUnitsPerCoin;
            
            return sellResourcesCount;
        }

        public async Task TrySellToReceiveCoins(ResourceType resourceType, int coinsReceived,
            IResourceBuildingReceivable receivable)
        {
            for (int i = 0; i < coinsReceived; i++)
            {
                int resourceUnitsPerCoin = GetResourceUnitsPerCoin(resourceType);
            
                bool shared = await _playerSharing.ShareResourcesWith(receivable, false, resourceUnitsPerCoin);

                if (!shared) 
                    await Task.Delay(2000);

                SellResource(resourceType, resourceUnitsPerCoin);
            }
        }

        private void SellResource(ResourceType resourceType, int sellResourceCount)
        {
            int coinsReceived = sellResourceCount / GetResourceUnitsPerCoin(resourceType);
            
            _gameResourceStorage.TryGiveResource(resourceType, sellResourceCount);
            _gameResourceStorage.TakeResource(ResourceType.Coin, coinsReceived);
        }

        private int GetResourceUnitsPerCoin(ResourceType type) => 
            _resourcesConfig.GetResourceUnitsPerCoin(type);
    }
}