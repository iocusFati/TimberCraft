using Infrastructure.Services;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public interface IGuidService : IService
    {
        string GetGuidFor(GameObject obj);
    }
}