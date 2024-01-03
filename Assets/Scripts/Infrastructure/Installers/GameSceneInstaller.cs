using Infrastructure.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
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