using Infrastructure.AssetProviderService;
using UnityEngine;
using Zenject;

namespace Infrastructure.States
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IAssets _assets;
        private readonly IInstantiator _container;

        public PlayerFactory(IAssets assets, IInstantiator container)
        {
            _assets = assets;
            _container = container;
        }

        public void CreatePlayer(Vector3 at)
        {
            Player player = _container.InstantiatePrefabResourceForComponent<Player>(AssetPaths.Player, at, Quaternion.identity,
                new GameObject("Holder").transform);

            player.transform.SetParent(null);
        }
    }
}