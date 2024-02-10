using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public interface IResourceBuildingReceivable
    {
        Transform ReceiveResourceTransform { get; }
    }
}