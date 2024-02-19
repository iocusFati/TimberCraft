using Gameplay.Buildings;
using Gameplay.Locations;
using Gameplay.Resource;
using Infrastructure.Services.Guid;
using UI.Mediator;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        public MainLocation mainLocation;
        public UIMediator uiMediator;
        public GuidService guidService;
        public GameCameraController gameCameraController;

        public override void InstallBindings()
        {
            BindMainLocation();

            BindUIMediator();
            
            BindGuidService();

            BindResourcesSelling();
            
            BindGameCameraController();
        }

        private void BindGameCameraController()
        {
            Container
                .Bind<IGameCameraController>()
                .FromInstance(gameCameraController)
                .AsSingle();
        }

        private void BindResourcesSelling()
        {
            Container
                .Bind<IResourcesSelling>()
                .To<ResourcesSelling>()
                .AsSingle();
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