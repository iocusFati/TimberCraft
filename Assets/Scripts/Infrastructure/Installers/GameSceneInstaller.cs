using Gameplay.Buildings;
using Gameplay.Environment.Locations;
using Gameplay.Player.ObstacleFade;
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
        public GameCameraStateController gameCameraController;
        public FadeObscurePlayerObjects fadeObscurePlayerObjects;

        public override void InstallBindings()
        {
            BindMainLocation();

            BindUIMediator();
            
            BindGuidService();

            BindResourcesSelling();
            
            BindGameCameraController();
            
            BindFadeObscurePlayerObjects();
        }

        private void BindFadeObscurePlayerObjects()
        {
            Container
                .Bind<FadeObscurePlayerObjects>()
                .FromInstance(fadeObscurePlayerObjects)
                .AsSingle();
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