using Cysharp.Threading.Tasks;
using Gameplay.Environment.Buildings;

namespace Gameplay.Resource
{
    public interface IResourcesSelling
    {
        public int GetMaxSellResourceCount(ResourceType type);
        UniTask TrySellToReceiveCoins(ResourceType resourceType, int coinsReceived,  IResourceBuildingReceivable receivable);
    }
}