using Gameplay.Resource;
using UnityEngine;

namespace Gameplay.Buildings
{
    public interface IResourceBuildingReceivable
    {
        Transform ReceiveResourceTransform { get; }
        ResourceType ConstructionResourceType { get; }
        
        void ReceiveResource(int resourceQuantity);
    }
}