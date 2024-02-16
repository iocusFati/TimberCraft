using System;

namespace Gameplay.Resource.ResourceStorage
{
    public interface IGameResourceStorage
    {
        int WoodCount { get; }
        int StoneCount { get; }

        event Action<int> OnCoinCountChanged;
        event Action<int> OnWoodCountChanged;
        event Action<int> OnStoneCountChanged;
        event Action<int> OnGoldCountChanged;

        bool TryGiveResource(ResourceType resourceType, int giveQuantity);
        void TakeResource(ResourceType resourceType, int takeQuantity);
        int GetResourceCountOfType(ResourceType type);
    }
}