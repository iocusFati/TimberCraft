using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;

namespace Gameplay.Resource.ResourceStorage
{
    public class GameResourceStorage : IGameResourceStorage, ISavedProgress
    {
        private Dictionary<ResourceType, int> _collectedResources = new()
        {
            { ResourceType.Coin, 0},
            { ResourceType.Wood, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Gold, 0 }
        };

        public int WoodCount => _collectedResources[ResourceType.Wood];
        public int StoneCount => _collectedResources[ResourceType.Stone];
        
        public event Action<int> OnCoinCountChanged;
        public event Action<int> OnWoodCountChanged;
        public event Action<int> OnStoneCountChanged;
        public event Action<int> OnGoldCountChanged;

        public GameResourceStorage(ISaveLoadService saveLoad)
        {
            saveLoad.Register(this);
        }

        public bool TryGiveResource(ResourceType resourceType, int giveQuantity)
        {
            int currentResources = _collectedResources[resourceType];
            
            if (currentResources <= 0)
                return false;
            
            int takeResources = currentResources >= giveQuantity ? giveQuantity : currentResources;

            _collectedResources[resourceType] -= takeResources;

            SendResourceCountChanged(resourceType);

            return true;
        }

        public void TakeResource(ResourceType resourceType, int takeQuantity)
        {
            _collectedResources[resourceType] += takeQuantity;
            
            SendResourceCountChanged(resourceType);
        }

        public int GetResourceCountOfType(ResourceType type) => 
            _collectedResources[type];

        public void OnProgressCouldNotBeLoaded(){ }

        public void LoadProgress(PlayerProgress progress)
        {
            _collectedResources = progress.CollectedResources;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.CollectedResources = _collectedResources;
        }

        private void SendResourceCountChanged(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Wood:
                    OnWoodCountChanged?.Invoke(_collectedResources[resourceType]);
                    break;
                case ResourceType.Stone:
                    OnStoneCountChanged?.Invoke(_collectedResources[resourceType]);
                    break;
                case ResourceType.Coin:
                    OnCoinCountChanged?.Invoke(_collectedResources[resourceType]);
                    break;
                case ResourceType.Gold:
                    OnGoldCountChanged?.Invoke(_collectedResources[resourceType]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }
    }
}