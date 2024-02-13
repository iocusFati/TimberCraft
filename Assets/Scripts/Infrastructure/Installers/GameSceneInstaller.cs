using Gameplay.Locations;
using Gameplay.Resource;
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
            
            BindGameResourceStorage();
        }

        private void BindGameResourceStorage()
        {
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