using Gameplay.Locations;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainLocation _mainLocation;

        public override void InstallBindings()
        {
            BindMainLocation();
        }

        private void BindMainLocation()
        {
            Container
                .Bind<MainLocation>()
                .FromInstance(_mainLocation)
                .AsSingle();
        }
    }
}