using Gameplay.Lumberjack;
using Infrastructure.AssetProviderService;
using UnityEngine;
using Utils;
using Zenject;

namespace Infrastructure.Factories.PlayerFactoryFolder
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IInstantiator _container;

        public PlayerFactory(IInstantiator container)
        {
            _container = container;
        }

        public void CreatePlayer(Vector3 at)
        {
            LumberjackBase player = _container.InstantiatePrefabResourceForComponent<LumberjackBase>(AssetPaths.Player,
                at, Quaternion.identity,
                new GameObject("Holder").transform);

            player.transform.SetParent(null);
        }
    }
}