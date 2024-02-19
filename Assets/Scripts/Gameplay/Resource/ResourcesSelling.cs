using Gameplay.Resource.ResourceStorage;

namespace Gameplay.Resource
{
    public class ResourcesSelling : IResourcesSelling
    {
        private readonly IGameResourceStorage _gameResourceStorage;
        
        private readonly int _resourceUnitsPerCoin;

        public ResourcesSelling(IGameResourceStorage gameResourceStorage)
        {
            _gameResourceStorage = gameResourceStorage;
        }

        public void SellAllPossibleOfType(ResourceType type, int resourceUnitsPerCoin)
        {
            int sellResourcesCount = GetSellResourceCount(type, resourceUnitsPerCoin, out var receiveCoins);

            _gameResourceStorage.TryGiveResource(type, sellResourcesCount);
            _gameResourceStorage.TakeResource(ResourceType.Coin, receiveCoins);
        }

        public int GetSellResourceCount(ResourceType type, int resourceUnitsPerCoin, out int receiveCoins)
        {
            receiveCoins = _gameResourceStorage.GetResourceCountOfType(type) / resourceUnitsPerCoin;
            int sellResourcesCount = receiveCoins * resourceUnitsPerCoin;
            
            return sellResourcesCount;
        }
    }
}