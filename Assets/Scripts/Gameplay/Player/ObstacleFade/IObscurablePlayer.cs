using UnityEngine;

namespace Gameplay.Player.ObstacleFade
{
    public interface IObscurablePlayer
    {
        MeshRenderer ObscureMesh { get; }
        GameObject MainGO { get; }
        GameObject DitherGO { get; }
        GameObject BlockerGO { get; }
    }
}