using System;
using Cinemachine;
using Gameplay.Lumberjack;
using Gameplay.Player;
using Infrastructure.AssetProviderService;
using UnityEngine;
using Utils;
using Zenject;

namespace Infrastructure.Factories.PlayerFactoryFolder
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IInstantiator _container;

        public event Action<Player> OnPlayerCreated;

        public PlayerFactory(IInstantiator container)
        {
            _container = container;
        }

        public void CreatePlayer(Vector3 at)
        {
            Player player = _container.InstantiatePrefabResourceForComponent<Player>(AssetPaths.Player,
                at, Quaternion.identity,
                new GameObject("Holder").transform);

            player.transform.SetParent(null);
            
            OnPlayerCreated?.Invoke(player);
        }
    }
}