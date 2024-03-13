using System.Threading.Tasks;
using Gameplay.Buildings;

namespace Gameplay.Resource
{
    public interface IResourcesSelling
    {
        public int GetMaxSellResourceCount(ResourceType type);
        Task TrySellToReceiveCoins(ResourceType resourceType, int coinsReceived,  IResourceBuildingReceivable receivable);
    }
}