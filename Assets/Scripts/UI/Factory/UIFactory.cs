using Base.UI.Entities.Windows;
using Infrastructure.AssetProviderService;
using Infrastructure.Services.StaticDataService;
using Infrastructure.States;
using UnityEngine;

namespace Base.UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly UIHolder _uiContainer;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticData;

        private Canvas _gameRoot;

        public UIFactory(UIHolder container,
            IAssets assets,
            IGameStateMachine gameStateMachine, 
            IStaticDataService staticData)
        {
            _uiContainer = container;
            _assets = assets;
            _gameStateMachine = gameStateMachine;
            _staticData = staticData;
        }

        public void CreateGameUIRoot() => 
            _gameRoot = CreateUIRoot("GameRoot");
        
        private TEntity CreateUIEntity<TEntity>(string path) where TEntity : Component, IUIEntity
        {
            if (_gameRoot == null) 
                CreateGameUIRoot();

            TEntity entity = _assets.Instantiate<TEntity>(path, _gameRoot.transform);
            _uiContainer.RegisterUIEntity(entity);

            return entity;
        }

        private Canvas CreateUIRoot(string name, int order = 0)
        {
            Canvas canvas = _assets.Instantiate<Canvas>(AssetPaths.UIRoot);
            canvas.name = name;
            canvas.sortingOrder = order;

            return canvas;
        }
    }
}