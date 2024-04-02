using Infrastructure.AssetProviderService;
using UI.Entities.HUD_Folder;
using UI.Entities.Windows;
using UnityEngine;
using Utils;
using Zenject;

namespace UI.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssets _assets;
        private readonly IInstantiator _instantiator;
        private UIHolder _uiContainer;

        private const int HudCanvasOrder = 1;

        private Canvas _gameRoot;

        [Inject]
        public UIFactory(IAssets assets, IInstantiator instantiator)
        {
            _assets = assets;
            _instantiator = instantiator;
        }

        public void Initialize(UIHolder uiHolder) => 
            _uiContainer = uiHolder;

        public void CreateGameUIRoot() => 
            _gameRoot = CreateUIRoot("GameRoot");

        public HUD CreateHUD()
        {
            Canvas hudCanvas = CreateUIRoot("HUD", HudCanvasOrder);

            HUD hud = CreateUIEntity<HUD>(AssetPaths.HUD, hudCanvas);

            return hud;
        }

        public void CreateCurtain() => 
            CreateUIEntity<Curtain>(AssetPaths.Curtain);

        private TEntity CreateUIEntity<TEntity>(string path, Canvas parent = null) where TEntity : Component, IUIEntity
        {
            TEntity entity = parent is not null 
                ? _instantiator.InstantiatePrefabResourceForComponent<TEntity>(path, parent.transform) 
                : _instantiator.InstantiatePrefabResourceForComponent<TEntity>(path);
            
            _uiContainer.RegisterUIEntity(entity);

            return entity;

            Canvas SetParentIfNull()
            {
                if (parent is not null) 
                    return parent;
                
                if (_gameRoot == null)
                    CreateGameUIRoot();
                else
                    parent = _gameRoot;

                return parent;
            }
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