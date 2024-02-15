using System.Collections.Generic;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;

namespace Gameplay.Resource
{
    public class GameResourceStorage : IGameResourceStorage, ISavedProgress
    {
        private Dictionary<ResourceType, int> _collectedResources = new()
        {
            { ResourceType.Wood, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Coin, 0}
        };

        public bool TryGiveResource(ResourceType resourceType, int giveQuantity)
        {
            int currentResources = _collectedResources[resourceType];
            
            if (currentResources <= 0)
                return false;
            
            int takeResources = currentResources >= giveQuantity ? giveQuantity : currentResources;

            _collectedResources[resourceType] -= takeResources;

            return true;
        }

        public void TakeResource(ResourceType resourceType, int takeQuantity)
        {
            _collectedResources[resourceType] += takeQuantity;
        }

        public void OnProgressCouldNotBeLoaded(){ }

        public void LoadProgress(PlayerProgress progress)
        {
            _collectedResources = progress.CollectedResources;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.CollectedResources = _collectedResources;
        }
    }
}