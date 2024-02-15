using Gameplay.Resource;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public interface IResourceBuildingReceivable
    {
        Transform ReceiveResourceTransform { get; }
        ResourceType ConstructionResourceType { get; }
        
        void ReceiveResource(int resourceQuantity);
    }
}