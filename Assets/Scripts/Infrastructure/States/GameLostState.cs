using Infrastructure.Services.Input;
using Infrastructure.States.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.States
{
    public class GameLostState : IState
    {
        private readonly IStateMachine _gameStateMachine;
        private readonly IInputService _inputService;
        private ICoroutineRunner _coroutineRunner;
        
        private readonly float _timeBeforeLose;
        private readonly float _cameraRotateDuration;
        
        public GameLostState(
            IStateMachine gameStateMachine)
            // IInputService inputService,
            // ICoroutineRunner coroutineRunner)
        {
            _gameStateMachine = gameStateMachine;
            // _inputService = inputService;
            // _coroutineRunner = coroutineRunner;
        }

        [Inject]
        public void Construct(IStateMachine gameStateMachine)
        {
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
        
        private void RestartGame()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(SceneManager.GetActiveScene().name);
        }
    }
}