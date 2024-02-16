using System;
using System.Collections;
using Gameplay.Resource;
using Gameplay.Resource.ResourceStorage;
using Infrastructure.AssetProviderService;
using Infrastructure.Factories;
using Infrastructure.Factories.PlayerFactoryFolder;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Pool;
using Infrastructure.Services.SaveLoad;
using Infrastructure.Services.StaticDataService;
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UI.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindSaveLoadService();
            
            BindUIHolder(out UIHolder uiHolder);

            BindStaticDataService();

            BindSceneLoader();

            BindStatesFactory();

            BindCoroutineRunner();

            BindGameStateMachine();

            BindAssetsService();

            BindFactories();

            BindInputService();

            BindUIFactory(uiHolder);

            BindPoolService();

            BindCacheService();

            BindPersistentProgressService();


            BindGameResourceStorage();
        }

        private void BindGameResourceStorage()
        {
            Container
                .Bind<IGameResourceStorage>()
                .To<GameResourceStorage>()
                .AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();
        }

        private void BindCacheService()
        {
            CacheService cacheService = new CacheService();

            Container
                .Bind<ICacheService>()
                .FromInstance(cacheService)
                .AsSingle();
            
            cacheService.Initialize();
        }

        private void BindStaticDataService()
        {
            StaticDataService staticData = new StaticDataService();

            Container
                .Bind<IStaticDataService>()
                .FromInstance(staticData)
                .AsSingle();
            
            staticData.Initialize();
        }
        
        private void BindPoolService()
        {
            PoolService poolService = new PoolService(Container.Resolve<IAssets>());
            
            Container
                .Bind<IPoolService>()
                .FromInstance(poolService)
                .AsSingle();
            
            poolService.Initialize();
        }

        private void BindUIHolder(out UIHolder uiHolder)
        {
            uiHolder = Container.Instantiate<UIHolder>(); 
            
            Container
                .Bind<IUIHolder>()
                .To<UIHolder>()
                .FromInstance(uiHolder)
                .AsSingle();
        }

        private void BindUIFactory(UIHolder uiHolder)
        {
            UIFactory uiFactory = Container.Instantiate<UIFactory>();
            uiFactory.Initialize(uiHolder);
            
            Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .FromInstance(uiFactory)
                .AsSingle();
        }

        private void BindInputService()
        {
            Container
                .Bind<IInputService>()
                .FromMethod(InputService)
                .AsSingle();
        }

        private void BindAssetsService()
        {
            Container
                .Bind<IAssets>()
                .To<AssetProvider>()
                .AsSingle();
        }
        
        private void BindSaveLoadService()
        {
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();
        }

        private void BindFactories()
        {
            // BindPlayerFactory();
            BindFactoriesHolder();

            void BindFactoriesHolder()
            {
                Container
                    .BindInterfacesAndSelfTo<FactoriesHolderService>()
                    .AsSingle();
            }

            void BindPlayerFactory()
            {
                Container
                    .Bind<IPlayerFactory>()
                    .To<PlayerFactory>()
                    .AsSingle();
            }
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<SceneLoader>()
                .AsSingle();
        }

        private void BindStatesFactory()
        {
            Container
                .BindInterfacesAndSelfTo<StatesFactory>()
                .AsSingle();
        }

        private void BindCoroutineRunner()
        {
            Container
                .Bind<ICoroutineRunner>()
                .FromInstance(this)
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container
                .Bind<IStateMachine>()
                .To<GameStateMachine>()
                .FromInstance(new GameStateMachine())
                .AsSingle();
        }

        public void DoAfter(Func<bool> condition, Action action) => 
            StartCoroutine(DoAfterCoroutine(condition, action));

        private IEnumerator DoAfterCoroutine(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(condition);

            Debug.Log("Action");
            action.Invoke();
        }

        private IInputService InputService() =>
            Application.isEditor
                ? new StandaloneInput()
                : new MobileInput();
    }
}
