using UnityEngine;

namespace Infrastructure.Services.Input
{
    public interface IInputService : IService
    {
        bool Tap();
        Vector2 Axis { get; }
    }
}