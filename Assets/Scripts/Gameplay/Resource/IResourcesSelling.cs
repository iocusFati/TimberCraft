namespace Gameplay.Resource
{
    public interface IResourcesSelling
    {
        public void SellAllPossibleOfType(ResourceType type, int resourceUnitsPerCoin);
        public int GetSellResourceCount(ResourceType type, int resourceUnitsPerCoin, out int receiveCoins);
    }
}