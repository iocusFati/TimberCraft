using Infrastructure.Factories;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string PlayerSpawnTag = "PlayerSpawn";
        
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly PlayerFactory _playerFactory;
        private readonly LocationFactory _locationFactory;
        private readonly SceneLoader _sceneLoader;

        private Vector3 _initialPoint;

        public LoadLevelState(IGameStateMachine gameStateMachine,
            ISaveLoadService saveLoadService,
            IFactoriesHolderService factoriesHolder, 
            SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            
            _playerFactory = factoriesHolder.PlayerFactory;
            _locationFactory = factoriesHolder.LocationFactory;
        }

        public void Enter(string sceneName)
        {
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                _sceneLoader.Load(sceneName, OnLoaded);
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

            _playerFactory.CreatePlayer(mainLocation.PlayerInitialPosition);
            
            _saveLoadService.InformReaders();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private void Reload()
        {
        }
    }
}