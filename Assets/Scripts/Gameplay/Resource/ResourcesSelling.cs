using Gameplay.Resource.ResourceStorage;
using Infrastructure.StaticData.ResourcesData;

namespace Gameplay.Resource
{
    public class ResourcesSelling
    {
        private readonly IGameResourceStorage _gameResourceStorage;
        
        private readonly int _resourceUnitsPerCoin;

        public ResourcesSelling(IGameResourceStorage gameResourceStorage, ResourcesConfig resourcesConfig)
        {
            _gameResourceStorage = gameResourceStorage;

            _resourceUnitsPerCoin = resourcesConfig.ResourceUnitsPerCoin;
        }

        public void SellAllPossibleOfType(ResourceType type)
        {
            int sellResourcesCount = GetSellResourceCount(type, out var receiveCoins);

            _gameResourceStorage.TryGiveResource(type, sellResourcesCount);
            _gameResourceStorage.TakeResource(ResourceType.Coin, receiveCoins);
        }

        public int GetSellResourceCount(ResourceType type, out int receiveCoins)
        {
            receiveCoins = _gameResourceStorage.GetResourceCountOfType(type) / _resourceUnitsPerCoin;
            int sellResourcesCount = receiveCoins * _resourceUnitsPerCoin;
            return sellResourcesCount;
        }
    }
}