using Gameplay.Bots.StateMachine.States;
using Gameplay.Locations;
using Infrastructure.Services.Guid;
using UI.Mediator;
using UnityEngine.Serialization;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public MainLocation mainLocation;
        public UIMediator uiMediator;
        public GuidService guidService;

        public override void InstallBindings()
        {
            BindMainLocation();

            BindUIMediator();
            
            BindGuidService();
        }

        private void BindGuidService()
        {
            Container
                .Bind<IGuidService>()
                .FromInstance(guidService)
                .AsSingle();
        }

        private void BindUIMediator()
        {
            Container
                .Bind<IUIMediator>()
                .FromInstance(uiMediator)
                .AsSingle();
        }

        private void BindMainLocation()
        {
            Container
                .Bind<MainLocation>()
                .FromInstance(mainLocation)
                .AsSingle();
        }
    }
}