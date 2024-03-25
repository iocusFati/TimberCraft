using System.Threading.Tasks;
using Gameplay.Buildings;
using Gameplay.Environment.Buildings;

namespace Gameplay.Resource
{
    public interface IResourcesSelling
    {
        public int GetMaxSellResourceCount(ResourceType type);
        Task TrySellToReceiveCoins(ResourceType resourceType, int coinsReceived,  IResourceBuildingReceivable receivable);
    }
}