using Gameplay.Resource;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public interface IResourceBuildingReceivable
    {
        Transform ReceiveResourceTransform { get; }
        ResourceType ResourceType { get; }
        
        void ReceiveResource(int resourceQuantity);
    }
}