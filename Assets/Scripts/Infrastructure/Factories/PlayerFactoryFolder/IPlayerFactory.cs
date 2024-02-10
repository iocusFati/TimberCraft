using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure.Factories.PlayerFactoryFolder
{
    public interface IPlayerFactory : IService
    {
        void CreatePlayer(Vector3 at);
    }
}