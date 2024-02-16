using Gameplay.Locations;
using Infrastructure.Factories;
using Infrastructure.Factories.BotFactoryFolder;
using Infrastructure.Factories.Location;
using Infrastructure.Factories.PlayerFactoryFolder;
using Infrastructure.Services.SaveLoad;
using Infrastructure.States.Interfaces;
using UI.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        
        private readonly PlayerFactory _playerFactory;
        private readonly LocationFactory _locationFactory;
        private readonly BotFactory _botFactory;
        private readonly IUIFactory _uiFactory;

        private Vector3 _initialPoint;

        public LoadLevelState(IStateMachine gameStateMachine,
            ISaveLoadService saveLoadService,
            IFactoriesHolderService factoriesHolder, 
            IUIFactory uiFactory, 
            SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            _uiFactory = uiFactory;

            _playerFactory = factoriesHolder.PlayerFactory;
            _locationFactory = factoriesHolder.LocationFactory;
            _botFactory = factoriesHolder.BotFactory;
        }

        public void Enter(string payload)
        {
            if (payload != SceneManager.GetActiveScene().name)
            {
                _sceneLoader.Load(payload, OnLoaded);
            }
            else
            {
                Reload();
            }
        }

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            MainLocation mainLocation = Object.FindObjectOfType<MainLocation>();
            mainLocation.Initialize();

            _playerFactory.CreatePlayer(mainLocation.PlayerInitialPosition);
            _uiFactory.CreateHUD();
            
            _saveLoadService.InformReaders();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void Reload()
        {
        }
    }
}