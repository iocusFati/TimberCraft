namespace Gameplay.Resource
{
    public interface IGameResourceStorage
    {
        bool TryGiveResource(ResourceType resourceType, int giveQuantity);
        void TakeResource(ResourceType resourceType, int takeQuantity);
    }
}