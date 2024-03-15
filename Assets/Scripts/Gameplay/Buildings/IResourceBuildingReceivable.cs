using Gameplay.Resource;
using UnityEngine;

namespace Gameplay.Buildings
{
    public interface IResourceBuildingReceivable
    {
        Transform ReceiveResourceTransform { get; }
        ResourceType ConstructionResourceType { get; }
        int NeededResources { get; }

        void ReceiveResource();
        void PromiseResource(int resourceShareQuantity);
    }
}